using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Touchgrass.Utils;

namespace Touchgrass.Tests
{
    public class TimeExtensionTests
    {
        [Theory]
        [InlineData(0, "00:00")]
        [InlineData(59, "00:59")]
        [InlineData(60, "01:00")]
        [InlineData(125, "02:05")]
        [InlineData(3599, "59:59")]
        [InlineData(3600, "01:00:00")]
        public void ToMinSecString_FormatsCorrectly(int seconds, string expected)
        {
            Assert.Equal(expected, seconds.ToMinSecString());
        }
    }
}
