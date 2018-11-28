using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace DrankReus_api.Models
{
    public class Order
    {
      public int Id { get; set; }
      public User User { get; set; }
      public DateTime OrderDate { get; set; }
      public int TaxPercentage { get; set; }
      public string TrackCode { get; set; }
      public string FirstName { get; set; }
      public string Prefix { get; set; }
      public string LastName { get; set; }
      public string Street { get; set; }
      public string BuildingNumber { get; set; }
      public string PostalCode { get; set; }
      public string Area { get; set; }
      public OrderProduct OrderProducts { get; set; }
    }
}
