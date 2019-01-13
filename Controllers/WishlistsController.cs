using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DrankReus_api.Data;
using DrankReus_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
            var wishlistProducts = (from w in db.Wishlists
                            from p in db.Product
                            where w.UserId == user.Id
                            where w.ProductId == p.Id
                            select new {wishId = w.Id, product = p }).ToArray();

            return Ok(wishlistProducts);
        }

        [HttpPost]
        public ActionResult Post([FromBody] NewWish newWish)
        {
            User user = GetClaimUser();
            bool productExists = db.Product.Any(x => x.Id == newWish.id);
            bool wishExists = db.Wishlists.Any(x => x.UserId == user.Id && x.ProductId == newWish.id);
            if(productExists && !wishExists)
            {
                Wishlist wish = new Wishlist()
                {
                    UserId = user.Id,
                    ProductId = newWish.id
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
            Wishlist deletewish = (from w in db.Wishlists
                                    where w.Id == id && w.UserId == user.Id
                                    select w).FirstOrDefault();
            db.Wishlists.Remove(deletewish);
            db.SaveChanges();
            return Ok();
        }

        private User GetClaimUser()
        {
            string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            return (from u in db.Users where u.Email == claimEmail select u).FirstOrDefault();
        }
    }

    public class NewWish
    {
        public int id { get; set; }
    }
}
