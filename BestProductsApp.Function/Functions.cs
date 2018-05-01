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

        public async Task LoadProducts([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer)
        {
            int maxId = 0, lastCount = 0;
            var allProducts = new List<Product>();
            do
            {
                var products = _db.Products.Skip(maxId).Take(1000).ToList();
                allProducts.AddRange(products);
                lastCount = products.Count();
                maxId += lastCount;
            } while (lastCount != 0);
            await cacheService.SetAsync("products", allProducts);
        }

        public async Task LoadProductsWithKey([TimerTrigger("0 */5 * * * *")]TimerInfo timerInfo)
        {
            int maxId = 0, lastCount = 0;
            do
            {
                var products = _db.Products.Skip(maxId).Take(1000).ToList();
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

        public async Task DeleteProduct([QueueTrigger("delete-product")]int productId)
        {
            string key = $"product-{productId}";
            var product = await _db.Products.FindAsync(productId);
            if (product == null) return;
            _db.Products.Remove(product);
            try
            {
                await _db.SaveChangesAsync();
                await cacheService.RemoveAsync(key);
            }
            catch
            {

            }

        }
    }
}
