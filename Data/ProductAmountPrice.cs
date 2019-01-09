using System;
using DrankReus_api.Models;

namespace DrankReus_api.Data
{

  class ProductAmountPrice
  {
    public Product ProductInfo { get; set; }
    public int Amount { get; set; }
    public decimal? Price { get; set; }
  }
    
}