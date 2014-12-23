using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bussedly.Models
{
    public class Prediction
    {
        public Bus Bus { get; set; }
        public TimeSpan DueTime { get; set; }

        public Prediction(Bus bus, string dueTime)
        {
            this.Bus = bus;
            this.DueTime = TimeSpan.Parse(dueTime);
        }
    }

    public class Route
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string[] Directions { get; set; }

        public Route(long id, string name, string[] directions)
        {
            this.Id = id;
            this.Name = name;
            this.Directions = directions;
        }
    }

    public class Bus
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public int Direction { get; set; }
        public Route Route { get; set; }

        public Bus(long id, string name, Position position, int direction)
        {
            this.Id = id;
            this.Name = name;
            this.Position = position;
            this.Direction = direction;
        }
    }
}