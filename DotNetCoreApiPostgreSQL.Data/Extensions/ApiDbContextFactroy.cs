using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreApiPostgreSQL.Data.Extensions
{
    public static class ApiDbContextFactroy
    {
        public static IServiceCollection AddApiDbContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApiDbContext>(
               options => options.UseNpgsql(Configuration.GetConnectionString("PostgresConnection"))
            );
            return services;
        }
    }
}
