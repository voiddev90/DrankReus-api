using System;
using System.ComponentModel.DataAnnotations;

namespace DrankReus_api.Models
{
  public class OrderProduct
  {
    public int Id { get; set; }
    
    [Required]
    public int OrderId { get; set; }
    
    public Order Order { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    public Product Product { get; set; }
    
    [Required]
    public decimal Price { get; set; }
  }
}