namespace Touchgrass.Utils
{
    public static class TimeExtensions
    {
        public static string ToMinSecString(this int seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            if (ts.Hours > 0)
            {
                return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            }
            else
            {
                return $"{ts.Minutes:D2}:{ts.Seconds:D2}";
            }
        }
    }
}
