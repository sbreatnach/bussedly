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

        public Route(string id, string name) : this(id, name, null)
        {
        }

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

        public Bus(string id, string name, Position position)
        {
            this.id = id;
            this.name = name;
            this.position = position;
        }
    }
}
