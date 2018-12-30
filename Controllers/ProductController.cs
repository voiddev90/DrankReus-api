using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DrankReus_api.Data;
using DrankReus_api.Models;
using ExtensionMethod;
using Microsoft.AspNetCore.Authorization;
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
        // [Route("")]
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
            if (Country.Length != 0)
            {
                result = result.Where(m => Country.Contains(m.CountryId.Value));
            }
            if (Category.Length != 0)
            {
                result = result.Where(m => Category.Contains(m.CategoryId.Value));
            }
            if (Brand.Length != 0)
            {
                result = result.Where(m => Brand.Contains(m.BrandId.Value));
            }
            if (products.Length != 0)
            {
                result = result.Where(p => products.Contains(p.Id));
            }
            if (price.Length != 0)
            {
                result = result.Where(p => p.Price >= price[0] && p.Price <= price[1]);
            }
            if (Percentage.Length != 0)
            {
                result = result.Where(p => p.Alcoholpercentage >= Percentage[0] && p.Alcoholpercentage <= Percentage[1]);
            }
            result = result.Where(p => p.Removed == false);
            return Ok(result.Select(p => new
            {
                p.Name,
                p.Id,
                p.Description,
                p.Price,
                p.Volume,
                p.Url,
                p.Alcoholpercentage,
                p.CategoryEntity,
                p.CountryEntity,
                p.BrandEntity,
                p.Removed
            }).GetPage(page_index, page_size, m => m.Price, ascending));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var res = await (from p in db.Product
                             where p.Id == id
                             where p.Removed == false
                             select new
                             {
                                 p.Name,
                                 p.Id,
                                 p.Description,
                                 p.Price,
                                 p.Volume,
                                 p.Url,
                                 p.Alcoholpercentage,
                                 p.CategoryEntity,
                                 p.CountryEntity,
                                 p.BrandEntity,
                                 p.Removed
                             }).FirstAsync();
            if (res == null) return NotFound();
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

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddProduct([FromBody] Product newProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await db.Product.AddAsync(newProduct);
            await db.SaveChangesAsync();
            return CreatedAtAction("GetProductById", new { id = newProduct.Id }, newProduct);
        }


        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateProduct(int id, Product updateInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.CategoryId = updateInfo.CategoryId;
            product.CountryId = updateInfo.CountryId;
            product.BrandId = updateInfo.BrandId;
            product.Name = updateInfo.Name;
            product.Description = updateInfo.Description;
            product.Price = updateInfo.Price;
            product.Volume = updateInfo.Volume;
            product.Alcoholpercentage = updateInfo.Alcoholpercentage;
            product.Url = updateInfo.Url;
            product.Inventory = updateInfo.Inventory;

            db.Product.Update(product);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveProduct(int id)
        {
            Product product = await db.Product.FindAsync(id);
            if (product == null || product.Removed)
            {
                return NotFound();
            }
            product.Removed = true;
            db.Product.Update(product);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}