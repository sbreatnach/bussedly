using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bussedly.Models
{
    public class Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Position(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Position position = obj as Position;
            if ((System.Object)position == null)
            {
                return false;
            }

            return (this.Latitude == position.Latitude) &&
                (this.Longitude == position.Longitude);
        }

        public bool Equals(Position position)
        {
            // If parameter is null return false:
            if ((object)position == null)
            {
                return false;
            }

            return (this.Latitude == position.Latitude) &&
                (this.Longitude == position.Longitude);
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32((this.Latitude * this.Longitude) * 1000000);
        }
    }
}