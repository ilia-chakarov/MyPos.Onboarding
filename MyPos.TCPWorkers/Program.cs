using MyPos.TCPWokrers.Workers;

namespace MyPos.TCPServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Warning);

            if(ConfigurationBinder.GetValue<bool>(builder.Configuration, "RunServer"))
                builder.Services.AddHostedService<TCPServerWorker>();

            if (ConfigurationBinder.GetValue<bool>(builder.Configuration, "RunClient"))
                builder.Services.AddHostedService<TCPClientWorker>();

            var host = builder.Build();
            host.Run();
        }
    }
}