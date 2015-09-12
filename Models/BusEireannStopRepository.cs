/*
Copyright (c) 2015, Glicsoft
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation and/or
other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors
may be used to endorse or promote products derived from this software without
specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NLog;

namespace bussedly.Models
{
    public class BusEireannRepository : IStopRepository, IBusRepository
    {
        private class Location
        {
            public int Latitude { get; set; }
            public int Longitude { get; set; }
        }

        private Logger logger;

        private LocalCache cache;
        private RawDataConverter jsObjectConverter;
        private ExtendedHttpClient client;
        private TimeUtilities timeUtils;

        static readonly TimeSpan CACHE_TIME_30SECS = new TimeSpan(0, 0, 30);
        static readonly TimeSpan CACHE_TIME_60SECS = new TimeSpan(0, 0, 60);
        static readonly TimeSpan CACHE_TIME_DAY = new TimeSpan(24, 0, 0);
        const string URL_BASE = "http://www.buseireann.ie/inc/proto";
        const string URL_STOPS = URL_BASE + "/stopPointTdi.php";
        const string URL_VEHICLES = URL_BASE + "/vehicleTdi.php";
        const string URL_STOP = URL_BASE + "/stopPassageTdi.php";
        const string URL_ROUTES = URL_BASE + "/routes.php";

        public BusEireannRepository()
        {
            this.logger = LogManager.GetCurrentClassLogger();
            this.cache = new LocalCache();
            this.client = new ExtendedHttpClient();
            this.timeUtils = new TimeUtilities();
            this.jsObjectConverter = new RawJsObjectDataConverter();
        }

        public IEnumerable<Stop> GetAllStopsByArea(Area area)
        {
            string curCacheKey = CacheKey.CreateKey("stops", area);
            var cacheData = this.cache.Get(curCacheKey);
            if (cacheData != null)
            {
                return (IEnumerable<Stop>) cacheData;
            }

            var values = new List<KeyValuePair<string, string>>();
            var northWestPosition = this.CreateLocationFromPosition(
                area.NorthWestPosition);
            var southEastPosition = this.CreateLocationFromPosition(
                area.SouthEastPosition);
            values.Add(new KeyValuePair<string, string>(
                "longitude_west", northWestPosition.Longitude.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "latitude_north", northWestPosition.Latitude.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "latitude_south", southEastPosition.Latitude.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "longitude_east", southEastPosition.Longitude.ToString()));
            
            var rawData = this.client.JsonGetAsync(URL_STOPS, values);

            var newStops = new Dictionary<string, Stop>();
            foreach (var rawStop in rawData.Content["stopPointTdi"].Values)
            {
                // skip the trash "foo" keys
                if (!(rawStop is Dictionary<string, dynamic>))
                {
                    continue;
                }
                var newPosition = this.CreatePositionFromLocation(
                    rawStop["latitude"].ToString(), rawStop["longitude"].ToString());
                var newStop = new Stop(
                    rawStop["duid"].ToString(),
                    rawStop["long_name"].ToString(),
                    newPosition,
                    rawStop["code"].ToString());
                this.cache.Set(
                    CacheKey.CreateKey(newStop), newStop, CACHE_TIME_DAY
                );
                newStops.Add(newStop.id, newStop);
            }

            if (newStops.Count() > 0)
            {
                this.cache.Set(
                    curCacheKey, newStops.Values, CACHE_TIME_DAY
                );
            }
            return newStops.Values;
        }

        public Stop GetStop(string id)
        {
            return (Stop)this.cache.Get(CacheKey.CreateStopKey(id));
        }

        public Dictionary<string, Route> GetRoutes()
        {
            string curCacheKey = CacheKey.CreateKey("routes");
            var cacheData = this.cache.Get(curCacheKey);
            if (cacheData != null)
            {
                return (Dictionary<string, Route>)cacheData;
            }

            var rawData = this.client.GetAsync(URL_ROUTES, this.jsObjectConverter);

            var newRoutes = new Dictionary<string, Route>();
            foreach (var rawRoute in rawData.Content["routeTdi"].Values)
            {
                // skip the trash "foo" keys
                if (!(rawRoute is Dictionary<string, dynamic>))
                {
                    continue;
                }
                var newRoute = new Route(
                    rawRoute["duid"].ToString(),
                    rawRoute["short_name"].ToString()
                );
                this.cache.Set(
                    CacheKey.CreateKey(newRoute), newRoute, CACHE_TIME_DAY
                );
                newRoutes.Add(newRoute.id, newRoute);
            }

            if (newRoutes.Count > 0)
            {
                this.cache.Set(curCacheKey, newRoutes, CACHE_TIME_DAY);
            }

            return newRoutes;
        }

        public Route GetRoute(string id)
        {
            return (Route)this.cache.Get(CacheKey.CreateRouteKey(id));
        }

        public IEnumerable<Prediction> GetStopPredictions(string id)
        {
            string curCacheKey = CacheKey.CreateKey("predictions", id);
            var cacheData = this.cache.Get(curCacheKey);
            if (cacheData != null)
            {
                return (IEnumerable<Prediction>)cacheData;
            }

            var stop = this.GetStop(id);
            if (stop == null)
            {
                throw new BussedException("Stop not found",
                                          HttpStatusCode.NotFound);
            }
            var curTime = this.timeUtils.GetCurrentUnixTimestampMillis();

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>(
                "stop_point", stop.id));
            values.Add(new KeyValuePair<string, string>(
                "_", (curTime + 600000).ToString()));

            var routes = this.GetRoutes();
            var rawData = this.client.JsonGetAsync(URL_STOP, values);
 
            var predictions = new List<Prediction>();
            foreach (var rawPrediction in rawData.Content["stopPassageTdi"].Values)
            {
                // skip the trash "foo" keys
                if (!(rawPrediction is Dictionary<string, dynamic>))
                {
                    continue;
                }
                var vehicleId = rawPrediction["vehicle_duid"]["duid"].ToString();
                var arrivalData = rawPrediction["arrival_data"];
                string actualPassageTime;
                arrivalData.TryGetValue("actual_passage_time", out actualPassageTime);
                var isActive = actualPassageTime != null;

                Int32 dueTime = 0;
                Bus bus = null;

                if (isActive)
                {
                    // bus is already on the move, so should be in cache
                    bus = this.GetBus(vehicleId);
                    dueTime = arrivalData["actual_passage_time_utc"].ToInt32();
                }
                if (bus == null)
                {
                    // bus not found in cache, or not on the move as yet
                    // so generate dummy bus for prediction
                    bus = new Bus(
                        "",
                        arrivalData["multilingual_direction_text"]["defaultValue"],
                        new Position(0, 0)
                    );
                    dueTime = rawPrediction["plannedTime"].ToInt32();
                }

                Route route;
                routes.TryGetValue(rawPrediction["route_duid"]["duid"].ToString(),
                                   out route);
                bus.route = route;
                predictions.Add(
                    new Prediction(
                        bus,
                        this.timeUtils.GetLocalTimeForTimestamp(dueTime),
                        isActive));
            }

            if (predictions.Count() > 0)
            {
                this.cache.Set(
                    curCacheKey, predictions, CACHE_TIME_30SECS
                );
            }
            return predictions;
        }

        public IEnumerable<Bus> GetAllBusesByArea(Area area)
        {
            string curCacheKey = CacheKey.CreateKey("buses", area);
            var cacheData = this.cache.Get(curCacheKey);
            if (cacheData != null)
            {
                return (IEnumerable<Bus>)cacheData;
            }

            var values = new List<KeyValuePair<string, string>>();
            var northWestPosition = this.CreateLocationFromPosition(
                area.NorthWestPosition);
            var southEastPosition = this.CreateLocationFromPosition(
                area.SouthEastPosition);
            values.Add(new KeyValuePair<string, string>(
                "longitude_west", northWestPosition.Longitude.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "latitude_north", northWestPosition.Latitude.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "latitude_south", southEastPosition.Latitude.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "longitude_east", southEastPosition.Longitude.ToString()));

            var rawData = this.client.JsonGetAsync(URL_VEHICLES, values);

            var newBuses = new Dictionary<string, Bus>();
            foreach (var rawBus in rawData.Content["vehicleTdi"].Values)
            {
                // skip the trash "foo" keys
                if (!(rawBus is Dictionary<string, dynamic>))
                {
                    continue;
                }
                var newPosition = this.CreatePositionFromLocation(
                    rawBus["latitude"].ToString(), rawBus["longitude"].ToString());
                var newBus = new Bus(
                    rawBus["duid"].ToString(),
                    "unknown",
                    newPosition);
                this.cache.Set(
                    CacheKey.CreateKey(newBus), newBus, CACHE_TIME_30SECS
                );
                newBuses.Add(newBus.id, newBus);
            }

            if (newBuses.Count() > 0)
            {
                this.cache.Set(
                    curCacheKey, newBuses.Values, CACHE_TIME_DAY
                );
            }
            return newBuses.Values;
        }

        public Bus GetBus(string id)
        {
            return (Bus)this.cache.Get(CacheKey.CreateBusKey(id));
        }

        private Position CreatePositionFromLocation(string latitude,
                                                    string longitude)
        {
            return this.CreatePositionFromLocation(
                int.Parse(latitude), int.Parse(longitude));
        }

        private Position CreatePositionFromLocation(int latitude,
                                                    int longitude)
        {
            // convert BusEireann's shoddy coords to actual WGS84 coords
            return new Position(latitude / 3600000.0, longitude / 3600000.0);
        }

        private BusEireannRepository.Location CreateLocationFromPosition(Position position)
        {
            // convert actual WGS84 coords to BusEireann's coord "system"
            var location = new BusEireannRepository.Location();
            location.Latitude = (int) Math.Round(position.latitude * 3600000.0);
            location.Longitude = (int) Math.Round(position.longitude * 3600000.0);
            return location;
        }
    }
}
