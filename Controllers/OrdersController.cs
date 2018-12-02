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
        public ActionResult<string> GetOrder(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] OrderProducts requestOrder)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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

            var productsAmount = (
                from p in db.Product
                from p_a in requestOrder.orderProductAmount
                where p_a.ProductId == p.Id
                select new {
                    product = p,
                    amount = p_a.Amount
                }
            ).ToArray();


            foreach (var productAmount in productsAmount)
            {
                order.OrderProducts.Add(new OrderProduct(){
                    Product = productAmount.product,
                    Price = productAmount.product.Price,
                    Amount = productAmount.amount
                });
                productAmount.product.Inventory -= productAmount.amount;
                db.Update(productAmount.product);
            }



            

            User user = GetClaimUser();
            if(user != null)
            {
                order.User = user;
                if(requestOrder.SaveAddress)
                {
                    user.Street = requestOrder.Street;
                    user.BuildingNumber = requestOrder.BuildingNumber;
                    user.PostalCode = requestOrder.PostalCode;
                    user.Area = requestOrder.Area;
                    db.Update(user);
                }
            }

            db.Add(order);
            await db.SaveChangesAsync();
            return CreatedAtAction("GetOrder", new {id = order.Id}, order.Id);

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
            if(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email) != null)
            {
                string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                return (from u in db.Users where u.Email == claimEmail select u).FirstOrDefault();
            }
            return null;
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
        public orderProductAmount[] orderProductAmount { get; set; }

        public bool SaveAddress { get; set; }
    }

    public class orderProductAmount
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
