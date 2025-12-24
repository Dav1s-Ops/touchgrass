namespace Touchgrass.Utils
{
    public static class TimeExtensions
    {
        public static string ToMinSecString(this int seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            return $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
    }
}
