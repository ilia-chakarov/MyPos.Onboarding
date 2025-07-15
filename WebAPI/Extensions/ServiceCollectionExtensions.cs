using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using WebAPI.Entities;
using WebAPI.Services;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

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

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();

            return services;
        }
    }
}
