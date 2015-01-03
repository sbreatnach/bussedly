using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace bussedly
{
    public class CommonExceptionLogger : ExceptionLogger
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override void Log(ExceptionLoggerContext context)
        {
            CommonExceptionLogger.logger.Error("Uncaught exception",
                                               context.Exception);
            base.Log(context);
        }
    }
}