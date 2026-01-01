using NetCoreAudio;
using System.Reflection;
using Touchgrass.Interfaces;

namespace Touchgrass.Services
{
    public class AlarmPlayer : IAlarmPlayer
    {
        private CancellationTokenSource? _cts;
        private Task? _loopTask;
        private string? _soundPath;

        private string ExtractEmbeddedSound()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Touchgrass.Resources.tng_nemesis_intruder_alert.mp3";

            var tempPath = Path.Combine(Path.GetTempPath(), "tng_nemesis_intruder_alert.mp3");

            if (File.Exists(tempPath)) return tempPath;

            using var resourceStream = assembly.GetManifestResourceStream(resourceName)
                                       ?? throw new FileNotFoundException("Embedded alarm sound not found.");

            using var fileStream = File.Create(tempPath);
            resourceStream.CopyTo(fileStream);

            return tempPath;
        }

        public void Start()
        {
            if (_loopTask != null) return; // already running

            _soundPath = ExtractEmbeddedSound();

            _cts = new CancellationTokenSource();

            _loopTask = Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    var player = new Player();
                    try
                    {
                        await player.Play(_soundPath);

                        // Wait for natural finish before next loop
                        while (player.Playing && !_cts.Token.IsCancellationRequested)
                        {
                            await Task.Delay(500, _cts.Token);
                        }
                    }
                    catch
                    {
                        // If Play fails, wait a bit and retry
                        await Task.Delay(1000, _cts.Token);
                    }
                    finally
                    {
                        try { await player.Stop(); } catch { }
                    }
                }
            }, _cts.Token);
        }

        public void Stop()
        {
            _cts?.Cancel();

            if (_loopTask != null)
            {
                try
                {
                    _loopTask.Wait(2000); // give it 2s to stop
                }
                catch { /* ignore */ }
            }

            _cts?.Dispose();
            _cts = null;
            _loopTask = null;

            if (File.Exists(_soundPath)) File.Delete(_soundPath); // clear from temp 'cache'
        }
    }
}