using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestProductsApp.API.Models.Products;
using BestProductsApp.Data;
using BestProductsApp.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BestProductsApp.API.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly BestProductsDbContext _db;
        public ProductsController(BestProductsDbContext db)
        {
            this._db = db;
        }

        [HttpPost]
        public IActionResult LoadData([FromBody]FilterModel model)
        {
            try
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
            catch (Exception)
            {
                throw;
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