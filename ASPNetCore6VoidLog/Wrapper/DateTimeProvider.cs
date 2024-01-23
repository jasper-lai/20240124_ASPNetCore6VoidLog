namespace ASPNetCore6VoidLog.Wrapper
{
    using System;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
