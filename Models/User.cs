using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace DrankReus_api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Een email adres is verplicht")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Een wachtwoord is verplicht")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string FirstName { get; set; }

        public string Prefix { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht")]
        public string LastName { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public string PostalCode { get; set; }

        public string Area { get; set; }
        
        public int DiscountPoints { get; set; }

        [Required(ErrorMessage = "Admin is niet gezet")]
        public bool Admin { get; set; } = false;

        public bool Removed { get; set; } = false;

        public object UserData()
        {
            return new {
                    Id = this.Id,
                    Email = this.Email,
                    FirstName = this.FirstName,
                    Prefix = this.Prefix,
                    LastName = this.LastName,
                    Street = this.Street,
                    BuildingNumber = this.BuildingNumber,
                    PostalCode = this.PostalCode,
                    Area = this.Area,
                    Admin = this.Admin,
                    DiscountPoints = this.DiscountPoints
                };
        }
    }
}
