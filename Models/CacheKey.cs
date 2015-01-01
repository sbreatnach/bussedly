using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bussedly.Models
{
    public class CacheKey
    {
        const string SEPARATOR = "_";

        public static string CreateKey(params string[] parts)
        {
            return String.Join(SEPARATOR, parts);
        }

        public static string CreateKey(string type, Area area)
        {
            return type + SEPARATOR + CreateKey(area);
        }

        public static string CreateKey(Area area)
        {
            return "area" + SEPARATOR +
                area.NorthWestPosition.latitude.ToString() + SEPARATOR +
                area.NorthWestPosition.longitude.ToString() + SEPARATOR +
                area.SouthEastPosition.latitude.ToString() + SEPARATOR +
                area.SouthEastPosition.longitude.ToString();
        }

        public static string CreateKey(Stop stop)
        {
            return CreateStopKey(stop.id);
        }

        public static string CreateStopKey(string id)
        {
            return "stop" + SEPARATOR + id;
        }

        public static string CreateKey(Stop stop, string direction)
        {
            return CreateKey(stop) + SEPARATOR +
                "direction" + SEPARATOR + direction;
        }

        public static string CreateKey(Route route)
        {
            return CreateRouteKey(route.id);
        }

        public static string CreateRouteKey(string id)
        {
            return "route" + SEPARATOR + id;
        }

        public static string CreateKey(Bus bus)
        {
            return CreateBusKey(bus.id);
        }

        public static string CreateBusKey(string id)
        {
            return "bus" + SEPARATOR + id;
        }
    }
}