using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bussedly.Models
{
    public class Stop
    {
        public string id { get; set; }
        public string name { get; set; }
        public Position position { get; set; }
        public string publicId { get; set; }

        public Stop(string id, string name, double latitude, double longitude,
                    string publicId)
            : this(id, name, new Position(latitude, longitude), publicId)
        {
        }

        public Stop(string id, string name, Position position, string publicId)
        {
            this.id = id;
            this.name = name;
            this.position = position;
            this.publicId = publicId;
        }
    }
}