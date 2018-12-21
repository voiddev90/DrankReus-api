using System;
using DrankReus_api.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using DrankReus_api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DrankReus_api.Helpers;

namespace DrankReus_api.Controllers
{
  [Route("api/[Controller]")]
  [ApiController]
  public class CartController : ControllerBase
  {
    private readonly WebshopContext db;
    private readonly decimal discountPercentage = 10m;
    public CartController(WebshopContext context) { db = context; }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult Post([FromBody] IdAmount[] productAmounts)
    {

      User user = GetClaimUser();
      var products =
      (from p in db.Product
       from pa in productAmounts
       where pa.Id == p.Id
       select new ProductAmountPrice()
       {
         ProductInfo = p,
         Amount = pa.Amount
       }).ToArray();

      ProductCalc pricing;
      if (user != null && user.DiscountPoints == 10)
      {
        pricing = new ProductCalc(discountPercentage);
      }
      else
      {
        pricing = new ProductCalc(0.00m);
      }
      pricing.calcPrice(products);

      return Ok(new {
        discountAmount = pricing.DiscountAmount,
        discountPercentage = pricing.DiscountPercentage,
        tax = pricing.Tax,
        totalPrice = pricing.TotalPrice,
        grandTotal = pricing.GrandTotal
      });


    }

    private User GetClaimUser()
    {
      if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email) != null)
      {
        string claimEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
        return (from u in db.Users where u.Email == claimEmail select u).FirstOrDefault();
      }
      return null;
    }

    public class IdAmount
    {
      public int Id { get; set; }
      public int Amount { get; set; }
    }
  }
}