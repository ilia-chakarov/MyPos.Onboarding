using System.Diagnostics;

namespace MyPos.WebAPI.BackgroundServices
{
    public class CPUMeterBacgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"CPU Usage: {cpuCounter.NextValue()}%");

                await Task.Delay(1000, stoppingToken);
            }

        }
    }
}
