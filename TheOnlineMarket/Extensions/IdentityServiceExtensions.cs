using DatabaseLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.Text;

namespace TheOnlineMarket.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentityCore<User>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
                .AddRoles<Role>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<Database>();



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var tokenkey = config["TokenKey"] ?? throw new Exception("TokenKey not found");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            return services;
        }

        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            var roles = new List<string> { "Seller", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role { Name = role });
                }
            }
        }
    }
}
