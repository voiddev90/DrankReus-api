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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using DrankReus_api.SeedData;
using System.Linq.Dynamic;

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
        [Route("Products")]
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
            return Ok(result.Select(m => m).GetPage(page_index,page_size, m => m.Id));
        }
        [HttpGet]
        [Route("{table_name}")]
        public IActionResult GetTableIdList(string table_name)
        {
            var tableList = db.GetType().GetProperty(table_name);
            IEnumerable<dynamic> res = (IEnumerable<dynamic>)tableList.GetValue(db);
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
}