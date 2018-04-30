using BestProductsApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestProductsApp
{
    public static class DIExtensions
    {
        public static void AddDataLayer(this IServiceCollection services, string connectionStrings)
        {
            services.AddDbContext<BestProductsDbContext>(options =>
            {
                options.UseNpgsql(connectionStrings, db => db.MigrationsAssembly("BestProductsApp.Data"));
            });
        }
    }
}
