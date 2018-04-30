using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestProductsApp.Data;
using BestProductsApp.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BestProductsApp.API.Controllers
{
    [Produces("application/json")]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly BestProductsDbContext _db;
        public ProductsController(BestProductsDbContext db)
        {
            this._db = db;
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