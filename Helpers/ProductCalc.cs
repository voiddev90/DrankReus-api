using System;
using DrankReus_api.Data;
using DrankReus_api.Models;

namespace DrankReus_api.Helpers
{

  class ProductCalc
  {

    public decimal TotalPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Tax { get; set; }
    public decimal GrandTotal { get; set; }

    public ProductCalc(decimal discountPercentage)
    {
      this.TotalPrice = 0.00m;
      this.DiscountPercentage = discountPercentage;
      this.DiscountAmount = 0.00m;
      this.Tax = 0.00m;
      this.GrandTotal = 0.00m;
    }

    public void calcPrice(ProductAmountPrice[] products)
    {

      foreach (var product in products)
      {
        if (product.Price.HasValue)
        {
          this.TotalPrice += product.Price.Value * product.Amount;
        }
        else
        {
          this.TotalPrice += product.ProductInfo.Price * product.Amount;
        }
      }
      if (this.DiscountPercentage > 0.00m)
      {
        this.DiscountAmount = Math.Round(((this.DiscountPercentage / 100m) * this.TotalPrice), 2, MidpointRounding.ToEven);
      }

      this.Tax = Math.Round((this.TotalPrice - this.DiscountAmount) * 0.21m, 2, MidpointRounding.ToEven);
      this.GrandTotal = this.TotalPrice - this.DiscountAmount;
    }

  }

}