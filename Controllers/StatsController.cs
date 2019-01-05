using System;
using Microsoft.AspNetCore.Mvc;
using DrankReus_api.Data;
using DrankReus_api.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DrankReus_api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class StatsController : Controller
    {
        private readonly WebshopContext db;
        public StatsController(WebshopContext context) {this.db = context;}


        [HttpGet, Route("products")]
        public async Task<ActionResult> getSoldProducts([FromQuery(Name = "month")] int month, [FromQuery(Name = "year")] int year)
        {
            var soldProducts = (from o in db.Orders
                          from o_p in db.OrderProducts
                          from p in db.Product
                          where o.OrderDate.Month == month && o.OrderDate.Year == year
                          where o.Id == o_p.OrderId
                          where o_p.ProductId == p.Id
                          group new {o_p.Amount, o_p.Price} by p into g
                          select new {id = g.Key, amount = g.Sum(x => x.Amount), price = (decimal)g.Sum(x => ((decimal)x.Price * x.Amount))}).ToArray();
            return Ok(soldProducts);
        }

    }
}