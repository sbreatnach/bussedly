using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bussedly.Models
{
    public class Position
    {
        public double latitude { get; set; }
        public double longitude { get; set; }

        public Position(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
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

            return (this.latitude == position.latitude) &&
                (this.longitude == position.longitude);
        }

        public bool Equals(Position position)
        {
            // If parameter is null return false:
            if ((object)position == null)
            {
                return false;
            }

            return (this.latitude == position.latitude) &&
                (this.longitude == position.longitude);
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32((this.latitude * this.longitude) * 1000000);
        }
    }
}