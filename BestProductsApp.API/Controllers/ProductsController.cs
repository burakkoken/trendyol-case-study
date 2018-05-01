using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestProductsApp.API.Models.Products;
using BestProductsApp.Data;
using BestProductsApp.Data.Entities;
using BestProductsApp.Services.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BestProductsApp.API.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly ICacheService _cacheService;
        private readonly BestProductsDbContext _db;
        public ProductsController(BestProductsDbContext db, ICacheService cacheService)
        {
            this._db = db;
            this._cacheService = cacheService;
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

        [HttpPost]
        public IActionResult Update([FromBody]Product product)
        {
            try
            {
                _db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
    }
}