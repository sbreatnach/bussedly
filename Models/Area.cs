using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bussedly.Models
{
    public class Area
    {
        public Position NorthWestPosition { get; set; }
        public Position SouthEastPosition { get; set; }

        public Area(double left, double right, double top, double bottom)
        {
            this.NorthWestPosition = new Position(top, left);
            this.SouthEastPosition = new Position(bottom, right);
        }

        public bool Contains(Position position)
        {
            return position.latitude <= this.NorthWestPosition.latitude &&
                position.latitude >= this.SouthEastPosition.latitude &&
                position.longitude >= this.NorthWestPosition.longitude &&
                position.longitude <= this.SouthEastPosition.longitude;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Area area = obj as Area;
            if ((System.Object)area == null)
            {
                return false;
            }

            return this.NorthWestPosition.Equals(area.NorthWestPosition) && 
                this.SouthEastPosition.Equals(area.SouthEastPosition);
        }

        public bool Equals(Area area)
        {
            // If parameter is null return false:
            if ((object)area == null)
            {
                return false;
            }

            return this.NorthWestPosition.Equals(area.NorthWestPosition) &&
                this.SouthEastPosition.Equals(area.SouthEastPosition);
        }

        public override int GetHashCode()
        {
            return this.NorthWestPosition.GetHashCode() ^ 
                this.SouthEastPosition.GetHashCode();
        }

    }
}