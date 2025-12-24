using Touchgrass.Interfaces;

namespace Touchgrass.Services
{
    public class CommandLineParser : ICommandLineParser
    {
        public IPomodoroConfig Parse(string[] args)
        {
            var config = new PomodoroConfig();

            if (args.Length == 0)
                return config; // run defaults

            for (int i = 0; i < args.Length; i += 2) // iterate through pairs, throw if invalid
            {
                if (i + 1 >= args.Length)
                    throw new ArgumentException("Invalid args: missing 'value' option. Ex: '--work 10'");

                string option = args[i].ToLower();
                if (!int.TryParse(args[i + 1], out int val) || val <= 0)
                    throw new ArgumentException("Invalid args: Value must be a postive int");

                switch (option)
                {
                    case "--work":
                        config.WorkDurationSeconds = val * 60;
                        break;
                    case "--break":
                        config.BreakDurationSeconds = val * 60;
                        break;
                    case "--cycles":
                        config.Cycles = val;
                        break;
                    case "--testing":
                        config.Cycles = 1;
                        config.BreakDurationSeconds = val;
                        config.WorkDurationSeconds = val;
                        break;
                    default:
                        throw new ArgumentException($"Unknown option: {option}");
                }
            }
            return config;
        }
    }
}
