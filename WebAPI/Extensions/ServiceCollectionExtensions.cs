using Elasticsearch.Net;
using ExternalApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.OpenApi.Models;
using MyPos.Configuration.Options;
using MyPos.WebAPI.BackgroundServices;
using MyPos.WebAPI.External.ClientServices;
using MyPos.WebAPI.External.ClientServices.Interfaces;
using MyPos.WebAPI.External.Handler;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using WebAPI.Entities;
using WebAPI.ExternalClients.Clients;
using WebAPI.ExternalClients.Clients.Interfaces;
using WebAPI.Options;
using WebAPI.Services;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureBacgroundServices(this IServiceCollection services, IConfiguration conf)
        {
            if (ConfigurationBinder.GetValue<bool>(conf, "CPUMeter"))
                services.AddHostedService<CPUMeterBackgroundService>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRegistrantService, RegistrantService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IUserAccessControlService, UserAccessControlService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IRegistrantsExtClientService, RegistrantsExtClientService>();
            services.AddScoped<IAuthExtClientService, AuthExtClientService>();

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            var swaggerOptions = configuration.GetSection("SwaggerOptions").Get<SwaggerOptions>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerOptions.Version,
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = swaggerOptions.Title,
                        Version = swaggerOptions.Version
                    });

                // JWT Bearer setup
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = swaggerOptions.JwtDescription,

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        jwtSecurityScheme,
                        Array.Empty<string>()
                    }
                });


                // Basic Auth setup
                var basicSecurityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = swaggerOptions.BasicDescription,

                    Reference = new OpenApiReference
                    {
                        Id = "basic",
                        Type = ReferenceType.SecurityScheme,
                    }
                };
                c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        basicSecurityScheme, Array.Empty<string>()
                    }
                });
            });


            return services;
        }

        public static IServiceCollection AddExternalHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<BearerTokenHandler>();

            var options = configuration.GetSection("IvoApi").Get<IvoApiSettings>();
            var baseUrl = options.BaseUrl;

            services.AddHttpClient<IvoApiClient>("IvoAPI", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            })
            .AddHttpMessageHandler<BearerTokenHandler>()
            .AddTypedClient((httpClient, sp) => new IvoApiClient(baseUrl, httpClient));

           

            return services;
        }




        public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder)
        {
            // Hardcoded here so that IT has freedom to use appsettings.json
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Application", "MyPos.Onboarding.WebAPI")
                // Elasticsearch Sink (for all logs except "Usage")
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(logEvent =>
                        logEvent.Properties.ContainsKey("LogType") &&
                        logEvent.Properties["LogType"].ToString() == "\"Usage\""
                    )
                    .WriteTo.Elasticsearch(
                        new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("https://10.80.128.112:9200/"))
                        {
                            AutoRegisterTemplate = false,
                            IndexFormat = "test-country-api",
                            CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                            ModifyConnectionSettings = conn => conn.BasicAuthentication("dev", "OYa4LbS#xV10")
                        }
                    )
                )
                // Main API Logs
                .WriteTo.Logger(
                lc => lc.Filter.ByExcluding(logEvent => 
                    logEvent.Properties.ContainsKey("LogType") &&
                    logEvent.Properties["LogType"].ToString() == "\"Usage\""
                    )
                .WriteTo.File(
                    "Logs/log-mypos-api-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [Machine: {MachineName}] {Message:lj}{NewLine}{Exception}"
                    )
                )
                // CPU Logs
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(logEvent => 
                        logEvent.Properties.ContainsKey("LogType") &&
                        logEvent.Properties["LogType"].ToString() == "\"Usage\""
                    ).WriteTo.File(
                        "Logs/Metrics/cpu-metrix-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                    )
                )
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => { Console.WriteLine(msg + Environment.NewLine); });

            hostBuilder.UseSerilog();

            return hostBuilder;
        }

        public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SwaggerOptions>(configuration.GetSection("SwaggerOptions"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<IvoApiSettings>(configuration.GetSection("IvoApi"));

            return services;
        }
    }
}
