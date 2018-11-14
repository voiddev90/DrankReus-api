using System;
using DrankReus_api.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using DrankReus_api.Models;
using Microsoft.AspNetCore.Authorization;

namespace DrankReus_api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly WebshopContext db;
        public CartController(WebshopContext context) {db = context;}

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Post([FromBody] IdAmount[] productAmounts)
        {
            System.Console.WriteLine(productAmounts);
            int[] ids = (from p in productAmounts
                        select p.id).ToArray();

            var products =
            (from p in db.Product
            where ids.Contains(p.Id)
            select p).ToList();
            decimal totalPrice = 0.00m;

            foreach (var product in products)
            {
                int amount = (from p in productAmounts where p.id == product.Id select p.amount).First();
                totalPrice += product.Price * amount;
            }
                            
            return Ok(new {
                // products = products,
                tax = Math.Round(totalPrice * 0.21m, 2, MidpointRounding.ToEven),
                Grandtotal = totalPrice
            });
        }
    }

    public class IdAmount
    {
        public int id { get; set; }
        public int amount { get; set; }
    }
}