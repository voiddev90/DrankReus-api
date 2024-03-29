using System;
using Microsoft.AspNetCore.Mvc;
using DrankReus_api.Data;
using DrankReus_api.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace DrankReus_api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class StatsController : Controller
    {
        private readonly WebshopContext db;
        public StatsController(WebshopContext context) {this.db = context;}


        [HttpGet, Route("products"), Authorize(Roles="Admin")]
        public ActionResult getSoldProducts([FromQuery(Name = "month")] int month, [FromQuery(Name = "year")] int year)
        {
            var soldProducts = (from o in db.Orders
                          from o_p in db.OrderProducts
                          from p in db.Product
                          where o.OrderDate.Month == month && o.OrderDate.Year == year
                          where o.Id == o_p.OrderId
                          where o_p.ProductId == p.Id
                          group new {o_p.Amount, o_p.Price} by p into g
                          select new {product = g.Key, amount = g.Sum(x => x.Amount), price = (decimal)g.Sum(x => ((decimal)x.Price * x.Amount))}).ToArray();
            return Ok(soldProducts);
        }

        [HttpGet, Route("Popular")]
        public ActionResult getPopularProducts(
            [FromQuery(Name = "month")] int month,
            [FromQuery(Name = "year")] int year)
        {
            return Ok(getSoldProducts(month,year));
        }

        [HttpGet, Route("productstock"), Authorize(Roles="Admin")]
        public async Task<ActionResult> getLowStock()
        {
            Product[] lowStockProducts = await db.Product.Where(p => p.Inventory <= 5).Where(p => p.Removed == false).ToArrayAsync();
            return Ok(lowStockProducts);
        }
    }
}