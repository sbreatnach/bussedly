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