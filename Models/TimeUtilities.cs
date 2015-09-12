using System;
using System.Globalization;
using NodaTime;

namespace bussedly.Models
{
    public class TimeUtilities
    {
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly IClock clock = SystemClock.Instance;
        private readonly IDateTimeZoneProvider timeZoneProvider
            = DateTimeZoneProviders.Tzdb;
        // defaulting to Irish culture for formatting purposes because laziness
        private readonly IFormatProvider formatter = new CultureInfo("en-IE");

        public long GetCurrentUnixTimestampMillis()
        {
            return (long)(clock.Now.Ticks / NodaConstants.TicksPerSecond);
        }

        public string GetLocalTimeForTimestamp(Int32 timestamp)
        {
            // defaulting to Irish timezone, cos I just don't care to
            // make it more flexible right now.
            var irishTimeZone = timeZoneProvider.GetZoneOrNull("Europe/Dublin");
            return this.GetLocalTimeForTimestamp(timestamp, irishTimeZone);
        }

        public string GetLocalTimeForTimestamp(Int32 timestamp, DateTimeZone timeZone)
        {
            var instant = Instant.FromSecondsSinceUnixEpoch(timestamp);
            var localDateTime = instant.InZone(timeZone);
            return localDateTime.ToString("HH:mm", formatter);
        }
    }
}