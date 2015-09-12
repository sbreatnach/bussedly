using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.Text;
using System.Web;

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
        private RawDataConverter jsonDataConverter = new RawJsonDictConverter();

        private String EncodeQueryParams(List<KeyValuePair<string, string>> queryParams)
        {
            var builder = new StringBuilder();
            var parts = new List<String>();
            foreach (var pair in queryParams)
            {
                builder.Append(HttpUtility.UrlEncode(pair.Key));
                builder.Append("=");
                builder.Append(HttpUtility.UrlEncode(pair.Value));
                parts.Add(builder.ToString());
                builder.Clear();
            }
            return String.Join("&", parts.ToArray());
        }

        public ExtendedHttpClient() : base()
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }

        public HttpData JsonGetAsync(string uri)
        {
            return this.JsonGetAsync(uri, null);
        }

        public HttpData JsonGetAsync(
            string uri,
            List<KeyValuePair<string, string>> queryParams)
        {
            return this.GetAsync(uri, this.jsonDataConverter, queryParams);
        }

        public HttpData GetAsync(
            string uri,
            RawDataConverter responseConverter)
        {
            return this.GetAsync(uri, responseConverter, null);
        }

        public HttpData GetAsync(
            string uri,
            RawDataConverter responseConverter,
            List<KeyValuePair<string, string>> queryParams)
        {
            var fullUri = uri;
            if (queryParams != null && queryParams.Count > 0)
            {
                fullUri = uri + "?" + this.EncodeQueryParams(queryParams);
            }
            var requestTask = this.GetAsync(fullUri);
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
            response.Content = responseConverter.Convert(contentTask.Result);
            return response;
        }
    }
}