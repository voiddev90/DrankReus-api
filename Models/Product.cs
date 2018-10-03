using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Text;
 
 namespace DrankReus_api.Models
 {
     public class Product
     {
         public int Id { get; set; }
         public int? CategoryId { get; set; }
         public int? CountryId { get; set; }
         public int? BrandId { get; set; }
         
         public string Name{ get; set; }
         public string Description { get; set; }
         public decimal Price{ get; set; }
         public string Volume { get; set; }
         public decimal Alcoholpercentage { get; set; }
         public string Url { get; set; }
         
         [System.ComponentModel.DataAnnotations.Schema.NotMapped]
         public string Category { get; set; }

         [System.ComponentModel.DataAnnotations.Schema.NotMapped]
         public string Brand { get; set; }

         public Brand BrandEntity { get; set; }

         public Category CategoryEntity { get; set; }
         
         [System.ComponentModel.DataAnnotations.Schema.NotMapped]
         public string Country { get; set; }
         public Country CountryEntity { get; set; }
     }
 }