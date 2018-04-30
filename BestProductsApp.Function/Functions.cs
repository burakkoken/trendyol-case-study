using BestProductsApp.Data;
using BestProductsApp.Data.Entities;
using BestProductsApp.Services.Cache;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestProductsApp.Function
{
    public class Functions
    {
        private readonly ICacheService cacheService;
        private readonly BestProductsDbContext _db;
        public Functions(ICacheService cacheService,
                         BestProductsDbContext db)
        {
            this.cacheService = cacheService;
            this._db = db;
        }

        public async Task LoadProducts([TimerTrigger("TimerInfo myTimer")]TimerInfo myTimer)
        {
            int maxId = 0, lastCount = 0;
            var allProducts = new List<Product>();
            do
            {
                var products = _db.Products.Skip(maxId).Take(1000);
                allProducts.AddRange(products);
                lastCount = products.Count();
                maxId += lastCount;
            } while (lastCount != 0);
            await cacheService.SetAsync("products", allProducts);
        }

        public async Task LoadProductsWithKey([TimerTrigger("TimerInfo myTimer")]TimerInfo myTimer)
        {
            int maxId = 0, lastCount = 0;
            do
            {
                var products = _db.Products.Skip(maxId).Take(1000);
                foreach (var product in products)
                {
                    await cacheService.SetAsync($"product-{product.Id}", product);
                }
                lastCount = products.Count();
                maxId += lastCount;
            } while (lastCount != 0);
        }

        public async Task UpdatedProduct([QueueTrigger("updated-product")]int productId)
        {
            string key = $"product-{productId}";
            var product = _db.Products.Find(productId);
            if (product == null) return;
            await cacheService.RemoveAsync(key);

            await cacheService.SetAsync(key, product);
        }

    }
}
