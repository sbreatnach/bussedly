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
