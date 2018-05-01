using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestProductsApp.API.Models.Products;
using BestProductsApp.Data;
using BestProductsApp.Data.Entities;
using BestProductsApp.Services.Cache;
using BestProductsApp.Services.Queue;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BestProductsApp.API.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly ICacheService _cacheService;
        private readonly IQueueService _queueService;
        private readonly BestProductsDbContext _db;
        public ProductsController(BestProductsDbContext db,
                                  ICacheService cacheService,
                                  IQueueService queueService)
        {
            this._db = db;
            this._cacheService = cacheService;
            this._queueService = queueService;
        }

        [Route("list")]
        [HttpPost]
        public IActionResult List([FromBody]FilterModel model)
        {
            var data = _db.Products.Skip(model.Start).Take(model.Length).ToList();

            return Json(new FilterModel()
            {
                Draw = model.Draw,
                RecordFiltered = data.Count,
                RecordsTotal = data.Count,
                Data = data
            });

        }

        [Route("list-cache")]
        [HttpPost]
        public IActionResult ListInCache([FromBody]FilterModel model)
        {
            var products = new List<Product>();
            var keys = _cacheService.GetAllKeys("product").OrderBy(x => x).Skip(model.Start).Take(model.Length).ToList();

            foreach (var key in keys)
            {
                products.Add(_cacheService.Get<Product>(key));
            }

            return Json(new FilterModel()
            {
                Draw = model.Draw,
                RecordFiltered = products.Count,
                RecordsTotal = _db.Products.Count(),
                Data = products
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody]int? id)
        {
            if (!id.HasValue) return BadRequest();
            var product = await _db.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id.Value);
            if (product == null) return NotFound();
            try
            {
                product.LastUpdatedTime = DateTime.Now;
                _db.Entry(product).State = EntityState.Modified;
                _queueService.AddQueue(BestProductsApp.Models.Services.QueueTypes.UpdateProduct, product.Id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

        [Route("fill")]
        [HttpGet]
        public async Task<IActionResult> Fill()
        {
            List<Product> products = new List<Product>();
            for (int i = 0; i < 1000000; i++)
                products.Add(new Product()
                {
                    Name = Faker.Name.FullName(),
                    Price = Faker.RandomNumber.Next(1000)
                });

            await _db.Products.AddRangeAsync(products);

            try
            {
                await _db.SaveChangesAsync();
                return Ok();

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            _queueService.AddQueue(BestProductsApp.Models.Services.QueueTypes.DeleteProduct, id.Value);
            return Ok(true);
        }
    }
}