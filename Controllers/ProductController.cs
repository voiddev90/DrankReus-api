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
using Newtonsoft.Json.Linq;

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
        [Route("ShowProduct")]
        public IActionResult ShowProduct()
        {
            var result = (from m in db.Product
                    where m.CountryId == 1
                    select new {m.Alcoholpercentage, m.Description, m.BrandEntity.Name})
                .GetPage(1, 100, m => m.Description);
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