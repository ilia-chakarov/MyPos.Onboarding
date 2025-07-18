using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using WebAPI.Entities;
using WebAPI.Services;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;
using Serilog;
using Microsoft.Extensions.Options;
using WebAPI.Options;
using WebAPI.ExternalClients.Clients.Interfaces;
using WebAPI.ExternalClients.Clients;
using ExternalApi;

namespace WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRegistrantService, RegistrantService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IUserAccessControlService, UserAccessControlService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IRegistrantsExtClientService, RegistrantsExtClientService>();

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MyPos API", Version = "v1" });

                // JWT Bearer setup
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter your JWT token below (without 'Bearer' prefix)",

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
                    Description = "Basic Authentication using username and password",

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

        public static IServiceCollection AddExternalHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IvoApiClient>((sp, cl) =>
            {
                var options = sp.GetRequiredService<IOptions<IvoApiSettings>>().Value;
                cl.BaseAddress = new Uri(options.BaseUrl);
            })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                })
                .AddTypedClient((httpCl, sp) =>
                {
                    var options = sp.GetRequiredService<IOptions<IvoApiSettings>>().Value;
                    var baseUrl = options.BaseUrl;
                    return new IvoApiClient(baseUrl, httpCl);
                });

            return services;
        }

        public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("Application", "MyPos.Onboarding.WebAPI")
                .WriteTo.File(
                    "Logs/log-mypos-api-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [Machine: {MachineName}] {Message:lj}{NewLine}{Exception}"
                ).CreateLogger();

            hostBuilder.UseSerilog();

            return hostBuilder;
        }

        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<IvoApiSettings>(configuration.GetSection("IvoApi"));

            return services;
        }
    }
}
