﻿/*
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
using System.Net;
using System.Net.Http;
using System.Web.Http;
using bussedly.Models;

namespace bussedly.Controllers
{
    public class StopsController : ApiController
    {
        private readonly IStopRepository _repository;

        public StopsController(IStopRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<Prediction> GetStopPredictions(string id)
        {
            IEnumerable<Prediction> predictions = null;
            try
            {
                predictions = _repository.GetStopPredictions(id);
            }
            catch(BussedException be)
            {
                throw new HttpResponseException(be.StatusCode);
            }
            if (predictions == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return predictions;
        }

        public IEnumerable<Stop> GetStopsByArea(double left, double right,
                                                double top, double bottom)
        {
            var area = new Area(left, right, top, bottom);
            try
            {
                return _repository.GetAllStopsByArea(area);
            }
            catch(BussedException be)
            {
                throw new HttpResponseException(be.StatusCode);
            }
        }
    }
}
