using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using NLog;

namespace bussedly.Models
{
    public class BusEireannRepository : IStopRepository
    {
        private class Location
        {
            public int Latitude { get; set; }
            public int Longitude { get; set; }
        }

        private Logger logger;

        private Dictionary<long, Route> routes;
        private Dictionary<long, Stop> stops;
        private Dictionary<long, Bus> buses;
        private ExtendedHttpClient client;
        private TimeUtilities timeUtils;
        private long lastBusRequestTimestamp;

        const string URL_BASE = "http://whensmybus.buseireann.ie/internetservice";
        const string URL_STOPS = URL_BASE + "/geoserviceDispatcher/services/stopinfo/stops";
        const string URL_VEHICLES = URL_BASE + "/geoserviceDispatcher/services/vehicleinfo/vehicles";
        const string URL_STOP = URL_BASE + "/services/passageinfo/stopPassages/stop";

        public BusEireannRepository()
        {
            this.logger = LogManager.GetCurrentClassLogger();
            this.routes = new Dictionary<long, Route>();
            this.stops = new Dictionary<long, Stop>();
            this.buses = new Dictionary<long, Bus>();
            this.client = new ExtendedHttpClient();
            this.timeUtils = new TimeUtilities();
        }

        public IEnumerable<Stop> GetAllStops()
        {
            return this.stops.Values;
        }

        public IEnumerable<Stop> GetAllStopsByArea(Area area)
        {
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

            this.stops.Clear();
            foreach (var rawStop in rawData.Content.stops)
            {
                var newPosition = this.CreatePositionFromLocation(
                    rawStop.latitude.ToString(), rawStop.longitude.ToString());
                var newStop = new Stop(
                    long.Parse(rawStop.id.ToString()),
                    rawStop.name.ToString(),
                    newPosition,
                    rawStop.shortName.ToString());
                this.stops.Add(newStop.Id, newStop);
            }

            return this.stops.Values;
        }

        public Stop GetStop(long id)
        {
            Stop stop;
            this.stops.TryGetValue(id, out stop);
            return stop;
        }

        public IEnumerable<Prediction> GetStopPredictions(long id)
        {
            return this.GetStopPredictions(id, "departure");
        }

        public IEnumerable<Prediction> GetStopPredictions(long id, string direction)
        {
            var stop = this.GetStop(id);
            var curTime = this.timeUtils.GetCurrentUnixTimestampMillis();

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>(
                "stop", stop.PublicId));
            values.Add(new KeyValuePair<string, string>(
                "mode", direction));
            values.Add(new KeyValuePair<string, string>(
                "startTime", curTime.ToString()));
            values.Add(new KeyValuePair<string, string>(
                "cacheBuster", (curTime + 600000).ToString()));

            var content = new FormUrlEncodedContent(values);
            var rawData = this.client.JsonPostSync(URL_STOP, content);

            foreach (var rawRoute in rawData.Content.routes)
            {
                var newRoute = new Route(
                    rawRoute.id, rawRoute.name, rawRoute.directions);
                this.routes.Add(rawRoute.id, newRoute);
            }

            var predictions = new List<Prediction>();
            foreach (var rawPrediction in rawData.Content.actual)
            {
                var bus = this.GetBus(rawPrediction.vehicleId);
                Route route;
                this.routes.TryGetValue(rawPrediction.routeId, out route);
                bus.route = route;
                predictions.Add(new Prediction(bus, rawPrediction.actualTime));
            }

            return predictions;
        }

        public Route GetRoute(long id)
        {
            Route route;
            this.routes.TryGetValue(id, out route);
            return route;
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return this.buses.Values;
        }

        public IEnumerable<Bus> GetAllBusesByArea(Area area)
        {
            if (this.lastBusRequestTimestamp == 0)
            {
                this.lastBusRequestTimestamp =
                    this.timeUtils.GetCurrentUnixTimestampMillis();
            }
            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>(
                "lastUpdate", this.lastBusRequestTimestamp.ToString()));

            var content = new FormUrlEncodedContent(values);
            var rawData = this.client.JsonPostSync(URL_STOPS, content);

            this.buses.Clear();
            foreach (var rawBus in rawData.Content.vehicles)
            {
                var newPosition = this.CreatePositionFromLocation(
                    rawBus.latitude, rawBus.longitude);
                var newBus = new Bus(
                    rawBus.id, rawBus.name, newPosition, rawBus.heading);
                this.buses.Add(rawBus.id, newBus);
            }

            this.lastBusRequestTimestamp =
                this.timeUtils.GetCurrentUnixTimestampMillis();
            return this.buses.Values;
        }

        public Bus GetBus(long id)
        {
            Bus bus;
            this.buses.TryGetValue(id, out bus);
            return bus;
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
            location.Latitude = (int) Math.Round(position.Latitude * 3600000.0);
            location.Longitude = (int) Math.Round(position.Longitude * 3600000.0);
            return location;
        }
    }
}