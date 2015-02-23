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
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using NLog;

namespace bussedly.Models
{
    public class HttpData
    {
        public HttpStatusCode StatusCode { get; set; }
        public dynamic Content { get; set; }
    }

    public class ExtendedHttpClient : HttpClient
    {
        private Logger logger;

        public ExtendedHttpClient() : base()
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }

        public HttpData JsonPostSync(string uri, HttpContent content)
        {
            var requestTask = this.PostAsync(uri, content);
            requestTask.Wait();
            var responseMessage = requestTask.Result;

            if (!responseMessage.IsSuccessStatusCode)
            {
                var message = "Unable to access Bus Eireann server: {0}";
                this.logger.Error(message, responseMessage.ReasonPhrase);
                throw new BussedException(
                    String.Format(message, responseMessage.ReasonPhrase),
                    HttpStatusCode.ServiceUnavailable);
            }

            var contentTask = responseMessage.Content.ReadAsStringAsync();
            contentTask.Wait();

            var response = new HttpData();
            response.StatusCode = responseMessage.StatusCode;
            response.Content = JsonConvert.DeserializeObject(contentTask.Result);
            return response;
        }
    }
}
