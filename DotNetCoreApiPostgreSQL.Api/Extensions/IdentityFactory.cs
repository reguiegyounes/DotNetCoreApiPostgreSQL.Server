using DotNetCoreApiPostgreSQL.Core.Models;
using DotNetCoreApiPostgreSQL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCoreApiPostgreSQL.Api.Extensions
{
    public static class IdentityFactory
    {
        public static IServiceCollection AddIdentityApp(this IServiceCollection services)
        {
            // Change Password Complexity
            services.AddIdentity<AppUser, IdentityRole>(options => {

                // No dublicate email
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApiDbContext>();
            return services;
        }
        public static IServiceCollection ConfigureWeakPassword(this IServiceCollection services)
        {
            // Change Password Complexity
            services.Configure<IdentityOptions>(options =>
            {
                // Very weak password
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });
            return services;
        }
    }
}
