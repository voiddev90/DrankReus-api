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
        public ActionResult Post([FromBody] int[] keys)
        {
            var products =
            (from p in db.Product
            where keys.Contains(p.Id)
            select p).ToList();
            decimal totalPrice = 0.00m;

            foreach (var product in products)
            {
                totalPrice += product.Price;
            }
                            
            return Ok(new {
                products = products,
                tax = Math.Round(totalPrice * 0.21m, 2, MidpointRounding.ToEven),
                Grandtotal = totalPrice
            });
        }
    }
}