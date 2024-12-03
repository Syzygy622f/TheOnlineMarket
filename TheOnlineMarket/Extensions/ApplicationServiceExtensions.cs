using BusinessLayer;
using DatabaseLayer;
using Microsoft.EntityFrameworkCore;

namespace TheOnlineMarket.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<Database>(options =>
            options.UseMySQL(config.GetConnectionString("DefaultConnection"))
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));


            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IUserAuthRepository, UserAuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJson_Token, Json_Token>();

            return services;
        }
    }
}
