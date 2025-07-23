using System.Diagnostics;

namespace MyPos.WebAPI.BackgroundServices
{
    public class CPUMeterBacgroundService : BackgroundService
    {
        private readonly ILogger<CPUMeterBacgroundService> _logger;
        public CPUMeterBacgroundService(ILogger<CPUMeterBacgroundService> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            while (!stoppingToken.IsCancellationRequested)
            {
                long gcMem = GC.GetTotalMemory(true);

                float usg = cpuCounter.NextValue();

                Console.WriteLine($"CPU Usage: {usg}%");

                _logger.LogInformation("{@LogType} Usage: {Usage}%", "CPU", usg);

                await Task.Delay(1000, stoppingToken);
            }

        }
    }
}
