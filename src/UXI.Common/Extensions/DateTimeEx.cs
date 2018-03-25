namespace System
{
    public static class DateTimeEx
    {
        public static readonly System.DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime FromUnixTimeStamp(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            return Epoch.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        public static long ToUnixTimeStamp(this DateTime time)
        {
            return time.ToUniversalTime().Subtract(Epoch).Seconds; 
        }
    }
}
