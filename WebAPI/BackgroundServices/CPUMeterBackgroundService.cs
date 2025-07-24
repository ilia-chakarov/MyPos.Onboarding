using System.Diagnostics;

namespace MyPos.WebAPI.BackgroundServices
{
    public class CPUMeterBackgroundService : BackgroundService
    {
        private readonly ILogger<CPUMeterBackgroundService> _logger;
        public CPUMeterBackgroundService(ILogger<CPUMeterBackgroundService> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            var process = Process.GetCurrentProcess();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TimeSpan lastCpuTime = process.TotalProcessorTime;

            while (!stoppingToken.IsCancellationRequested)
            {
                process.Refresh();
                TimeSpan currentCpuTime = process.TotalProcessorTime;
                double elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
                double cpuUsedMs = (currentCpuTime - lastCpuTime).TotalMilliseconds;
                double appCpuUsage = (cpuUsedMs / (elapsedMs * Environment.ProcessorCount)) * 100;
                lastCpuTime = currentCpuTime;
                stopwatch.Restart();

                double gcMem = GC.GetTotalMemory(true) / (1024.0 * 1024.0);

                float usg = cpuCounter.NextValue();

                Console.WriteLine($"CPU Usage: {usg}%");

                _logger.LogInformation("{@LogType}: Total CPU {SystemCpy:F2}% | App CPU {AppCpuUsg:F2}% | App Heap {HeapMemory:F2}MB",
                        "Usage", usg, appCpuUsage, gcMem);

                await Task.Delay(1000, stoppingToken);
            }

        }
    }
}
