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
      private readonly int discountPercentage = 10;
      public OrdersController(WebshopContext context){db = context;}

        [Authorize]
        [HttpGet]
        public ActionResult GetOrders()
        {
            User user = GetClaimUser();
            var orders = (from o in db.Orders
                        let orderProduct = (
                            from p in db.Product
                            from o_p in db.OrderProducts
                            where o_p.ProductId == p.Id && o.Id == o_p.OrderId
                            select new {
                                product = p,
                                price = o_p.Price,
                                amount = o_p.Amount
                            }
                        ).ToArray()
                        where o.UserId == user.Id || user.Admin == true
                        orderby o.Id descending
                        select new {
                            order = o.filterUser(),
                            orderProduct = orderProduct
                        }).ToArray();
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult GetOrder(int id)
        {
            User user = GetClaimUser();
            var order = (from o in db.Orders
                        let orderProduct = (
                            from p in db.Product
                            from o_p in db.OrderProducts
                            where o_p.ProductId == p.Id && o.Id == o_p.OrderId
                            select new {
                                product = p,
                                price = o_p.Price,
                                amount = o_p.Amount
                            }
                        ).ToArray()
                        where o.Id == id && (o.UserId == user.Id || user.Admin == true)
                        select new {
                            order = o.filterUser(),
                            orderProduct = orderProduct
                        }).ToArray();
            return Ok(order);
        }

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
            order.OrderDate = DateTime.Now;
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
                }
                if(user.DiscountPoints == 10)
                {
                    order.DiscountPercentage = this.discountPercentage;
                    user.DiscountPoints = 0;
                }
                else
                {
                    user.DiscountPoints++;
                }
                db.Update(user);
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
