using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DrankReus_api.Data;
using DrankReus_api.Models;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace DrankReus_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

      private readonly WebshopContext db;
      public OrdersController(WebshopContext context) {db = context;}

        // [HttpGet]
        // public ActionResult Get()
        // {
        //     // var user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value; <--- TO GET THE EMAIL FROM THE USER THAT IS IN THE TOKEN
        //     // return Ok(new String[] { "value1", "value2" });
        // }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Post([FromBody] OrderProducts requestOrder)
        {
            Order order = new Order();
            order.OrderStatus = OrderStatusEnum.Ordered;
            order.OrderDate = new DateTime();
            order.TaxPercentage = 21;
            order.Email = requestOrder.Email;
            order.FirstName = requestOrder.FirstName;
            order.Prefix = requestOrder.Prefix;
            order.LastName = requestOrder.LastName;
            order.Street = requestOrder.Street;
            order.BuildingNumber = requestOrder.BuildingNumber;
            order.PostalCode = requestOrder.PostalCode;
            order.Area = requestOrder.Area;
            order.OrderProducts = new List<OrderProduct>();

            List<Product> products =
            (from p in db.Product
            where requestOrder.ProductIds.Contains(p.Id)
            select p).ToList();

            foreach (Product product in products)
            {
                order.OrderProducts.Add(new OrderProduct(){
                    Product = product,
                    Price = product.Price
                });
            }

            db.Add(order);
            db.SaveChanges();
            return Ok();

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private User GetClaimUser()
        {
            string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            return (from u in db.Users where u.Email == claimEmail select u).FirstOrDefault();
        }
    }

    public class OrderProducts
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }

        public string Prefix { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Street { get; set; }
        [Required]
        public string BuildingNumber { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public int[] ProductIds { get; set; }
    }
}
