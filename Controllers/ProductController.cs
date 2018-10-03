using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DrankReus_api.Data;
using DrankReus_api.Models;
using ExtensionMethod;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
namespace DrankReus_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly WebshopContext db;
        public ProductController(WebshopContext context) {db = context;}
        
        [HttpGet]
        [Route("InitialInsert")]
        public IActionResult GetActionResult()
        {
            using (StreamReader d = new StreamReader(Directory.GetCurrentDirectory() + "/SeedData/Drank.json"))
            using (StreamReader b = new StreamReader(Directory.GetCurrentDirectory() + "/SeedData/DrankBrand.json")) 
            using (StreamReader co = new StreamReader(Directory.GetCurrentDirectory() + "/SeedData/DrankCountry.json"))
            using (StreamReader ca = new StreamReader(Directory.GetCurrentDirectory() + "/SeedData/DrankCategory.json"))
            {
                if (!db.Product.Any())
                {
                    string djson = d.ReadToEnd();
                    string bjson = b.ReadToEnd();
                    string cojson = co.ReadToEnd();
                    string cajson = ca.ReadToEnd();
                    List<Product> drank = JsonConvert.DeserializeObject<List<Product>>(djson);
                    List<Category> category = JsonConvert.DeserializeObject<List<Category>>(cajson);
                    List<Country> country = JsonConvert.DeserializeObject<List<Country>>(cojson);
                    List<Brand> brand = JsonConvert.DeserializeObject<List<Brand>>(bjson);
                    foreach (var entry in brand)
                    {
                        Brand productBrand = new Brand
                        {
                            Name = entry.Name
                        };
                        db.Add(productBrand);
                    }
                    db.SaveChanges();
                    foreach (var entry in category)
                    {
                        Category productCategory = new Category
                        {
                            Name = entry.Name
                        };
                        db.Add(productCategory);
                    }
                    db.SaveChanges();
                    foreach (var entry in country)
                    {
                        Country productCountry = new Country
                        {
                            Name = entry.Name
                        };
                        db.Add(productCountry);
                    }
                    db.SaveChanges();
                    foreach (var drink in drank)
                    {
                        Product prod = new Product
                        {
                            Alcoholpercentage = drink.Alcoholpercentage,
                            Description = drink.Description,
                            Name = drink.Name,
                            Price = drink.Price,
                            Url = drink.Url,
                            Volume = drink.Volume,
                            CountryEntity =
                                db.Country.Where(n => n.Name == drink.Country).Select(x => x).FirstOrDefault(),
                            CategoryEntity = db.Category.Where(n => n.Name == drink.Category).Select(x => x)
                                .FirstOrDefault(),
                            BrandEntity = db.Brand.Where(n => n.Name == drink.Brand).Select(x => x)
                                .FirstOrDefault()
                        };
                        db.Add(prod);
                    }
                    db.SaveChanges();
                    db.Dispose();
                    return Ok("added");
                }
            }

            return Ok("nothing to add");
        }

        [HttpGet]
        [Route("ShowProduct")]
        public IActionResult ShowProduct()
        {
            var result = (from m in db.Product
                where m.CountryId == 1
                select new {m.Alcoholpercentage, m.Description, m.Brand}).GetPage(1,100,m => m.Description);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("GetPages/{page_index}/{page_size}")]
        public IActionResult GetPageCount(IQueryable result, int page_index, int page_size)
        {
            var res = db.Product.GetPage(page_index, page_size, a => a);
            if (res == null) return NotFound();
            return Ok(res);
        }
    }
}