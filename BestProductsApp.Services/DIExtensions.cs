using BestProductsApp.Models.Services;
using BestProductsApp.Services.Cache;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestProductsApp
{
    public static class DIExtensions
    {
        public static void AddServices(this IServiceCollection services, string redisConstr, string storageConstr, string containerName)
        {
            services.Configure<CacheServiceOptions>(option =>
            {
                option.ConnectionString = redisConstr;
            });
            services.Configure<AzureStorageOptions>(options =>
            {
                options.ConnectionString = storageConstr;
                options.ContainerName = containerName;
            });
            services.AddScoped<ICacheService, CacheService>();
        }
    }
}
