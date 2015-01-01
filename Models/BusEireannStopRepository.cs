using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
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

        private ObjectCache cache;
        private ExtendedHttpClient client;
        private TimeUtilities timeUtils;
        private long lastBusRequestTimestamp;

        static readonly TimeSpan CACHE_SHORT_TIME = new TimeSpan(0, 0, 60);
        static readonly TimeSpan CACHE_LONG_TIME = new TimeSpan(24, 0, 0);
        const string URL_BASE = "http://whensmybus.buseireann.ie/internetservice";
        const string URL_STOPS = URL_BASE + "/geoserviceDispatcher/services/stopinfo/stops";
        const string URL_VEHICLES = URL_BASE + "/geoserviceDispatcher/services/vehicleinfo/vehicles";
        const string URL_STOP = URL_BASE + "/services/passageInfo/stopPassages/stop";

        public BusEireannRepository()
        {
            this.logger = LogManager.GetCurrentClassLogger();
            this.cache = MemoryCache.Default;
            this.client = new ExtendedHttpClient();
            this.timeUtils = new TimeUtilities();
        }

        public IEnumerable<Stop> GetAllStopsByArea(Area area)
        {
            string curCacheKey = CacheKey.CreateKey("stops", area);
            var cacheData = this.cache.Get(curCacheKey);
            if (cacheData != null)
            {
                this.logger.Debug("GetAllStopsByArea::cache hit");
                return (IEnumerable<Stop>) cacheData;
            }
            this.logger.Debug("GetAllStopsByArea::cache miss");

            var values = new List<KeyValuePair<string, string>>();
            var northWestPosition = this.CreateLocationFromPosition(
                area.NorthWestPosition);
            var southEastPosition = this.CreateLocationFromPosition(
                area.SouthEastPosition);
            values.Add(new KeyValuePair<string, string>(
                "left", northWestPosition.Longitude.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "top", northWestPosition.Latitude.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "bottom", southEastPosition.Latitude.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "right", southEastPosition.Longitude.ToString()));

            var content = new FormUrlEncodedContent(values);
            var rawData = this.client.JsonPostSync(URL_STOPS, content);

            var newStops = new Dictionary<string, Stop>();
            foreach (var rawStop in rawData.Content.stops)
            {
                var newPosition = this.CreatePositionFromLocation(
                    rawStop.latitude.ToString(), rawStop.longitude.ToString());
                var newStop = new Stop(
                    rawStop.id.ToString(),
                    rawStop.name.ToString(),
                    newPosition,
                    rawStop.shortName.ToString());
                this.cache.Set(
                    CacheKey.CreateKey(newStop), newStop,
                    new DateTimeOffset(DateTime.UtcNow + CACHE_LONG_TIME)
                );
                newStops.Add(newStop.id, newStop);
            }

            this.cache.Set(
                curCacheKey, newStops.Values,
                new DateTimeOffset(DateTime.UtcNow + CACHE_LONG_TIME)
            );
            return newStops.Values;
        }

        public Stop GetStop(string id)
        {
            return (Stop)this.cache.Get(CacheKey.CreateStopKey(id));
        }

        public IEnumerable<Prediction> GetStopPredictions(string id)
        {
            return this.GetStopPredictions(id, "departure");
        }

        public IEnumerable<Prediction> GetStopPredictions(string id, string direction)
        {
            string curCacheKey = CacheKey.CreateKey("predictions", id, direction);
            var cacheData = this.cache.Get(curCacheKey);
            if (cacheData != null)
            {
                this.logger.Debug("GetStopPredictions::cache hit");
                return (IEnumerable<Prediction>)cacheData;
            }
            this.logger.Debug("GetStopPredictions::cache miss");

            var stop = this.GetStop(id);
            if (stop == null)
            {
                throw new BussedException("Stop not found",
                                          HttpStatusCode.NotFound);
            }
            var curTime = this.timeUtils.GetCurrentUnixTimestampMillis();

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>(
                "stop", stop.publicId));
            values.Add(new KeyValuePair<string, string>(
                "mode", direction));
            values.Add(new KeyValuePair<string, string>(
                "startTime", curTime.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "cacheBuster", (curTime + 600000).ToString()));

            var content = new FormUrlEncodedContent(values);
            var rawData = this.client.JsonPostSync(URL_STOP, content);

            var newRoutes = new Dictionary<string, Route>();
            foreach (var rawRoute in rawData.Content.routes)
            {
                var newRoute = new Route(
                    rawRoute.id.ToString(), rawRoute.name.ToString(), rawRoute.directions);
                this.cache.Set(
                    CacheKey.CreateKey(newRoute), newRoute,
                    new DateTimeOffset(DateTime.UtcNow + CACHE_LONG_TIME)
                );
                newRoutes.Add(rawRoute.id, newRoute);
            }

            var predictions = new List<Prediction>();
            foreach (var rawPrediction in rawData.Content.actual)
            {
                var bus = this.GetBus(rawPrediction.vehicleId.ToString());
                Route route;
                newRoutes.TryGetValue(rawPrediction.routeId.ToString(),
                                      out route);
                bus.route = route;
                predictions.Add(
                    new Prediction(bus, rawPrediction.actualTime.ToString()));
            }

            this.cache.Set(
                curCacheKey, predictions,
                new DateTimeOffset(DateTime.UtcNow + CACHE_SHORT_TIME)
            );
            return predictions;
        }

        public Route GetRoute(string id)
        {
            return (Route)this.cache.Get(CacheKey.CreateRouteKey(id));
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            string curCacheKey = "buses";
            var cacheData = this.cache.Get(curCacheKey);
            if (cacheData != null)
            {
                this.logger.Debug("GetAllBuses::cache hit");
                return (IEnumerable<Bus>)cacheData;
            }
            this.logger.Debug("GetAllBuses::cache miss");

            if (this.lastBusRequestTimestamp == 0)
            {
                this.lastBusRequestTimestamp =
                    this.timeUtils.GetCurrentUnixTimestampMillis();
            }
            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>(
                "lastUpdate", this.lastBusRequestTimestamp.ToString()));

            var content = new FormUrlEncodedContent(values);
            var rawData = this.client.JsonPostSync(URL_VEHICLES, content);

            var newBuses = new Dictionary<string, Bus>();
            foreach (var rawBus in rawData.Content.vehicles)
            {
                if (rawBus.isDeleted != null)
                {
                    continue;
                }
                var newPosition = this.CreatePositionFromLocation(
                    rawBus.latitude.ToString(), rawBus.longitude.ToString());
                var newBus = new Bus(
                    rawBus.id.ToString(),
                    rawBus.name.ToString(),
                    newPosition,
                    int.Parse(rawBus.heading.ToString()));
                if (newBuses.ContainsKey(newBus.id))
                {
                    this.logger.Warn("Duplicated bus with ID={0}", newBus.id);
                    continue;
                }
                this.cache.Set(
                    CacheKey.CreateKey(newBus), newBus,
                    new DateTimeOffset(DateTime.UtcNow + CACHE_SHORT_TIME)
                );
                newBuses.Add(newBus.id, newBus);
            }

            this.lastBusRequestTimestamp =
                this.timeUtils.GetCurrentUnixTimestampMillis();
            this.cache.Set(
                curCacheKey, newBuses.Values,
                new DateTimeOffset(DateTime.UtcNow + CACHE_SHORT_TIME)
            );
            return newBuses.Values;
        }

        public IEnumerable<Bus> GetAllBusesByArea(Area area)
        {
            string curCacheKey = CacheKey.CreateKey("buses", area);
            var cacheData = this.cache.Get(curCacheKey);
            if (cacheData != null)
            {
                this.logger.Debug("GetAllBusesByArea::cache hit");
                return (IEnumerable<Bus>)cacheData;
            }
            this.logger.Debug("GetAllBusesByArea::cache miss");

            var allBuses = this.GetAllBuses();
            var filteredBuses = new List<Bus>();
            foreach (var bus in allBuses)
            {
                if (area.Contains(bus.position))
                {
                    filteredBuses.Add(bus);
                }
            }

            this.cache.Set(
                curCacheKey, filteredBuses,
                new DateTimeOffset(DateTime.UtcNow + CACHE_SHORT_TIME)
            );
            return filteredBuses;
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