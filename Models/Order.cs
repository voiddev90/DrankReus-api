using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace DrankReus_api.Models
{
    public class Order
    {
      public int Id { get; set; }
      
      public int? UserId { get; set; }
      
      public User User { get; set; }

      public OrderStatusEnum OrderStatus { get; set; }
      
      [Required]
      public DateTime OrderDate { get; set; }
      
      [Required]
      public int TaxPercentage { get; set; }
      
      public string TrackCode { get; set; }

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
      
      public List<OrderProduct> OrderProducts { get; set; }

      public object filterUser()
      {
        return new {
          Id = this.Id,
          OrderStatus = this.OrderStatus,
          OrderDate = this.OrderDate,
          TaxPercentage = this.TaxPercentage,
          TrackCode = this.TrackCode,
          Email = this.Email,
          FirstName = this.FirstName,
          LastName = this.LastName,
          Street = this.Street,
          BuildingNumber = this.BuildingNumber,
          PostalCode = this.PostalCode,
          Area = this.Area
        };
      }
    }

    public enum OrderStatusEnum
    {
      Ordered,
      Paid,
      Packaging,
      Shipped,
      Delivered
    }
}
