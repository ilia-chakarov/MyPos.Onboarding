using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Middleware;
using WebAPI.Extensions;
using WebAPI.AutoMapper;


namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerDocumentation();
            
            builder.Services.AddAppSettings(builder.Configuration);

            builder.Services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );


            builder.Services.AddAutoMapper(typeof(AccountProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(WalletProfile).Assembly);


            builder.Services.AddApplicationServices();

            builder.Services.AddJwtAuthentication(builder.Configuration);
            
           
            builder.Host.ConfigureSerilog();


            // Ivo client
            builder.Services.AddExternalHttpClients();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
