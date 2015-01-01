using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bussedly.Models
{
    public class Prediction
    {
        public Bus bus { get; set; }
        public TimeSpan dueTime { get; set; }
        public bool active { get; set; }

        public Prediction(Bus bus, string dueTime, bool active)
        {
            this.bus = bus;
            this.dueTime = TimeSpan.Parse(dueTime);
            this.active = active;
        }
    }

    public class Route
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<String> directions { get; set; }

        public Route(string id, string name, List<String> directions)
        {
            this.id = id;
            this.name = name;
            this.directions = directions;
        }
    }

    public class Bus
    {
        public string id { get; set; }
        public string name { get; set; }
        public Position position { get; set; }
        public int direction { get; set; }
        public Route route { get; set; }

        public Bus(string id, string name, Position position, int direction)
        {
            this.id = id;
            this.name = name;
            this.position = position;
            this.direction = direction;
        }
    }
}