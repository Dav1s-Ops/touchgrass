using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touchgrass
{
    public static class TimeExtensions
    {
        public static string ToMinSecString(this int seconds, bool showHours = true, string separator = ":")
        {
            var time = TimeSpan.FromSeconds(seconds);
            var hours = (int)time.TotalHours;

            if (showHours && hours > 0)
            {
                return $"{hours}h {time:mm\\:ss}";
            }

            return $"{time:mm\\:ss}";
        }
    }
}
