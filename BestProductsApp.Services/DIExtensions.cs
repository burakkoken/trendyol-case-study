using BestProductsApp.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestProductsApp
{
    public static class DIExtensions
    {
        public static void AddServices(this IServiceCollection services,string connectionString)
        {
            services.Configure<CacheServiceOptions>(option =>
            {
                option.ConnectionString = connectionString;
            });
        }
    }
}
