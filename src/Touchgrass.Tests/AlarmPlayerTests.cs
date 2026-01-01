using FluentAssertions;
using System.Reflection;
using Touchgrass.Services;

namespace Touchgrass.Tests
{
    public class AlarmPlayerServiceTests
    {
        private readonly AlarmPlayer _service;

        public AlarmPlayerServiceTests()
        {
            _service = new AlarmPlayer();
        }

        [Fact]
        public void ExtractEmbeddedSound_Should_Create_Temp_File()
        {
            var path = _service.TestOnly_ExtractEmbeddedSound();

            File.Exists(path).Should().BeTrue();
            new FileInfo(path).Length.Should().BeGreaterThan(1000);
        }

        [Fact]
        public async Task Start_Should_Begin_Playback_And_Loop()
        {
            var path = _service.TestOnly_ExtractEmbeddedSound();

            _service.Start();

            await Task.Delay(500);

            _service.TestOnly_IsLoopRunning().Should().BeTrue();

            _service.Stop();
        }

        [Fact]
        public async Task Stop_Should_Cancel_Loop_Quickly()
        {
            _service.Start();
            await Task.Delay(300);

            _service.Stop();

            await Task.Delay(100);
            _service.TestOnly_IsLoopRunning().Should().BeFalse();
        }

        [Fact]
        public async Task Start_Stop_Start_Should_Work_Multiple_Times()
        {
            // First cycle
            _service.Start();
            await Task.Delay(300);
            _service.TestOnly_IsLoopRunning().Should().BeTrue();
            _service.Stop();
            await Task.Delay(100);
            _service.TestOnly_IsLoopRunning().Should().BeFalse();

            // Second cycle
            _service.Start();
            await Task.Delay(300);
            _service.TestOnly_IsLoopRunning().Should().BeTrue();
            _service.Stop();

            // Final state
            _service.TestOnly_IsLoopRunning().Should().BeFalse();
        }

        [Fact]
        public void Start_When_Already_Running_Should_Be_Idempotent()
        {
            _service.Start();
            var firstTask = _service.TestOnly_GetLoopTask();

            _service.Start(); // second call

            _service.TestOnly_GetLoopTask().Should().BeSameAs(firstTask); // same task, not new

            _service.Stop();
        }
    }

    public static class AlarmPlayerTestExtensions
    {
        private static readonly FieldInfo _ctsField = typeof(AlarmPlayer)
            .GetField("_cts", BindingFlags.NonPublic | BindingFlags.Instance)!;

        private static readonly FieldInfo _loopTaskField = typeof(AlarmPlayer)
            .GetField("_loopTask", BindingFlags.NonPublic | BindingFlags.Instance)!;

        public static string TestOnly_ExtractEmbeddedSound(this AlarmPlayer service)
        {
            var method = typeof(AlarmPlayer)
                .GetMethod("ExtractEmbeddedSound", BindingFlags.NonPublic | BindingFlags.Instance)!;
            return (string)method.Invoke(service, null)!;
        }

        public static bool TestOnly_IsLoopRunning(this AlarmPlayer service)
        {
            var task = (Task?)_loopTaskField.GetValue(service);
            return task != null && !task.IsCompleted && !task.IsFaulted && !task.IsCanceled;
        }

        public static Task? TestOnly_GetLoopTask(this AlarmPlayer service)
        {
            return (Task?)_loopTaskField.GetValue(service);
        }
    }
}