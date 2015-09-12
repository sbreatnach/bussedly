/*
Copyright (c) 2015, Glicsoft
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation and/or
other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors
may be used to endorse or promote products derived from this software without
specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
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
