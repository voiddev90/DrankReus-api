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
        [FromQuery(Name = "size")]int page_size)
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
            }).GetPage(page_index,page_size, m => m.Id));
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

        [HttpGet]
        [Route("GetPages/{page_index}/{page_size}")]
        public IActionResult GetPageCount(int page_index, int page_size)
        {
            var res = db.Product.GetPage(page_index, page_size, a => a);
            if (res == null) return NotFound();
            return Ok(res);
        }
    }
    public class RefinedProduct{
        public Product product { get; set; }
    }
}