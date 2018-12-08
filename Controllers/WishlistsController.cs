using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DrankReus_api.Data;
using DrankReus_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrankReus_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly WebshopContext db;
        public WishlistsController(WebshopContext context) { db = context; }

        [HttpGet]
        public ActionResult Get()
        {
            User user = GetClaimUser();
            Product[] wishlistProducts = (from w in db.Whishlists
                            from p in db.Product
                            where w.UserId == user.Id
                            where w.ProductId == p.Id
                            select p).ToArray();

            return Ok(wishlistProducts);
        }

        [HttpPost]
        public ActionResult Post([FromBody] int productId)
        {
            User user = GetClaimUser();
            bool productExists = db.Product.Any(x => x.Id == productId);
            bool wishExists = db.Whishlists.Any(x => x.UserId == user.Id && x.ProductId == productId);
            if(productExists && !wishExists)
            {
                Whishlist wish = new Whishlist()
                {
                    UserId = user.Id,
                    ProductId = productId
                };
                db.Add(wish);
                db.SaveChanges();
                return StatusCode(201);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            User user = GetClaimUser();
            Whishlist deletewish = (from w in db.Whishlists
                                    where w.Id == id && w.UserId == user.Id
                                    select w).FirstOrDefault();
            db.Whishlists.Remove(deletewish);
            db.SaveChanges();
            return Ok();
        }

        private User GetClaimUser()
        {
            string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            return (from u in db.Users where u.Email == claimEmail select u).FirstOrDefault();
        }
    }
}
