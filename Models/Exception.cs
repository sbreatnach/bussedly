using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace bussedly.Models
{
    public class BussedException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public BussedException(string message) : this(message, HttpStatusCode.InternalServerError)
        {
        }

        public BussedException(string message, HttpStatusCode statusCode) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}