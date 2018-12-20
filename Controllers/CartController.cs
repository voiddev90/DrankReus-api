using System;
using DrankReus_api.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using DrankReus_api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
      System.Console.WriteLine(productAmounts);
      int[] ids = (from p in productAmounts
                   select p.Id).ToArray();

      var products =
      (from p in db.Product
       where ids.Contains(p.Id)
       select p).ToList();
      decimal totalPrice = 0.00m;

      foreach (var product in products)
      {
        int amount = (from p in productAmounts where p.Id == product.Id select p.Amount).First();
        totalPrice += Math.Round(product.Price * amount, 2, MidpointRounding.ToEven);
      }

      User user = GetClaimUser();
      decimal discountAmount = 0.00m;
      if (user != null && user.DiscountPoints == 10)
      {
          discountAmount = Math.Round(((this.discountPercentage / 100m) * totalPrice), 2, MidpointRounding.ToEven);
      }

        return Ok(new
        {
          // products = products,
          tax = Math.Round((totalPrice - discountAmount) * 0.21m, 2, MidpointRounding.ToEven),
          discountAmount = discountAmount,
          totalPrice = totalPrice,
          grandtotal = totalPrice - discountAmount
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