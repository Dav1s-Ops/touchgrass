using Touchgrass.Services;

namespace Touchgrass.Tests
{
    public class CommandLineParserTests
    {
        private readonly CommandLineParser _parser = new();

        [Fact]
        public void Parse_Defaults_WhenNoArgs()
        {
            var args = Array.Empty<string>();
            var config = _parser.Parse(args);
            Assert.Equal(4, config.Cycles);
            Assert.Equal(25 * 60, config.WorkDurationSeconds);
            Assert.Equal(5 * 60, config.BreakDurationSeconds);
        }

        [Fact]
        public void Parser_CustomWorkDuration()
        {
            var args = new[] { "--work", "15" };
            var config = _parser.Parse(args);
            Assert.Equal(15 * 60, config.WorkDurationSeconds);
            Assert.Equal(5 * 60, config.BreakDurationSeconds);
            Assert.Equal(4, config.Cycles);
        }

        [Fact]
        public void Parse_CustomBreakDuration()
        {
            var args = new[] { "--break", "10" };
            var config = _parser.Parse(args);
            Assert.Equal(25 * 60, config.WorkDurationSeconds);
            Assert.Equal(10 * 60, config.BreakDurationSeconds);
            Assert.Equal(4, config.Cycles);
        }

        [Fact]
        public void Parse_CustomCycles()
        {
            var args = new[] { "--cycles", "3" };
            var config = _parser.Parse(args);
            Assert.Equal(25 * 60, config.WorkDurationSeconds);
            Assert.Equal(5 * 60, config.BreakDurationSeconds);
            Assert.Equal(3, config.Cycles);
        }

        [Fact]
        public void Parse_TestingMode()
        {
            var args = new[] { "--testing", "30" };
            var config = _parser.Parse(args);
            Assert.Equal(1, config.Cycles);
            Assert.Equal(30, config.WorkDurationSeconds);
            Assert.Equal(30, config.BreakDurationSeconds);
        }

        [Fact]
        public void Parse_MultipleOptions()
        {
            var args = new[] { "--work", "20", "--break", "7", "--cycles", "5" };
            var config = _parser.Parse(args);
            Assert.Equal(20 * 60, config.WorkDurationSeconds);
            Assert.Equal(7 * 60, config.BreakDurationSeconds);
            Assert.Equal(5, config.Cycles);
        }

        [Fact]
        public void Parse_ThrowsOnInvalidOption()
        {
            var args = new[] { "--nope", "10" };
            Assert.Throws<ArgumentException>(() => _parser.Parse(args));
        }

        [Fact]
        public void Parse_ThrowsOnMissingValue()
        {
            var args = new[] { "--work" };
            Assert.Throws<ArgumentException>(() => _parser.Parse(args));
        }

        [Fact]
        public void Parse_ThrowsOnNonPositiveValue()
        {
            var args = new[] { "--work", "-123" };
            Assert.Throws<ArgumentException>(() => _parser.Parse(args));
        }

        [Fact]
        public void Parse_ThrowsOnNonIntegerValue()
        {
            var args = new[] { "--work", "abc" };
            Assert.Throws<ArgumentException>(() => _parser.Parse(args));
        }
    }
}
