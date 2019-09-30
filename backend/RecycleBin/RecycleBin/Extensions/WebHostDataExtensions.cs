using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecycleBin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecycleBin.Extensions
{
    public static class WebHostDataExtensions
    {
        public static IWebHost MigrateDatabase<TContext>(this IWebHost host) where TContext : ApplicationDbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<TContext>();
                context.Database.Migrate();
            }

            return host;
        }
    }
}
