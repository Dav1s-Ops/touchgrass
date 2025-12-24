namespace Touchgrass.Interfaces
{
    public interface ICommandLineParser
    {
        IPomodoroConfig Parse(string[] args);
    }
}
