using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bussedly.Models
{
    public class Stop
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public string PublicId { get; set; }

        public Stop(long id, string name, double latitude, double longitude,
                    string publicId)
            : this(id, name, new Position(latitude, longitude), publicId)
        {
        }

        public Stop(long id, string name, Position position, string publicId)
        {
            this.Id = id;
            this.Name = name;
            this.Position = position;
            this.PublicId = publicId;
        }
    }
}