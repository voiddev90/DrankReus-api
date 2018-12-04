using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DrankReus_api.Data;
using DrankReus_api.Models;
using ExtensionMethod;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrankReus_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly WebshopContext db;

        public ProductController(WebshopContext context)
        {
            db = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult GetFilteredProducts(
        [FromQuery(Name = "Country")]int[] Country,
        [FromQuery(Name = "Category")]int[] Category,
        [FromQuery(Name = "Brand")]int[] Brand,
        [FromQuery(Name = "index")]int page_index,
        [FromQuery(Name = "size")]int page_size,
        [FromQuery(Name = "products")] int[] products,
        [FromQuery(Name = "Percentage")] int[] Percentage,
        [FromQuery(Name = "Price")] int[] price,
        [FromQuery(Name = "Ascending")] bool ascending)
        {
            var result = db.Product.Select(m => m);
            if(Country.Length != 0){
                result = result.Where(m => Country.Contains(m.CountryId.Value));
            }
             if(Category.Length != 0){
                result = result.Where(m => Category.Contains(m.CategoryId.Value));
            }
            if(Brand.Length != 0){
                result = result.Where(m => Brand.Contains(m.BrandId.Value));
            }
            if (products.Length != 0){
                result = result.Where(p => products.Contains(p.Id));
            }
            if(price.Length != 0){
                result = result.Where(p => p.Price >= price[0] && p.Price <= price[1]);
            }
            if(Percentage.Length != 0){
                result = result.Where(p => p.Alcoholpercentage >= Percentage[0] && p.Alcoholpercentage <= Percentage[1]);
            }
            return Ok(result.Select(p => new {
                p.Name,
                p.Id,
                p.Description,
                p.Price,
                p.Volume,
                p.Url,
                p.Alcoholpercentage,
                p.CategoryEntity,
                p.CountryEntity,
                p.BrandEntity
            }).GetPage(page_index,page_size, m=> m.Price, ascending));
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetProductById(int id){
            var res = from p in db.Product
                        where p.Id == id
                        select new{
                            p.Id,
                            p.Description,
                            p.Price,
                            p.Volume,
                            p.Url,
                            p.Alcoholpercentage,
                            p.CategoryEntity,
                            p.CountryEntity,
                            p.BrandEntity};
            return Ok(res);
        }

        [HttpPut]
        [Route("Purchased")]
        public IActionResult ManageInventory(
        [FromBody]int[] productIds)
        {
            Dictionary<int,int> productcount = new Dictionary<int, int>();
            foreach (var i in productIds.GroupBy(x => x))
            {
                productcount.Add(i.Key,i.Count());
            }
            var result = (from p in db.Product
            where productIds.Contains(p.Id)
            select p).ToList();
           foreach (var item in result)
           {
               int count;
               productcount.TryGetValue(item.Id,out count);
               item.Inventory = item.Inventory - count;
           }
            db.SaveChanges();
            return Ok(201);
        }

        [HttpGet]
        [Route("Test")]
        public IActionResult GetPossibleIds(
        [FromQuery(Name = "Product")]int[] Products){
            var Brand  = db.Product.Where(m => Products.Contains(m.Id))
            .GroupBy(x => x.BrandEntity)
            .Select(m => new {
                Key = m.Key.Id,
                val = m.Count()
            }).Distinct();

            var Country  = db.Product.Where(m => Products.Contains(m.Id))
            .GroupBy(x => x.CountryEntity)
            .Select(m => new {
                Key = m.Key.Id,
                val = m.Count()
            }).Distinct();
            return Ok((Brand,Country));
        }
    }
}