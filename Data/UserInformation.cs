using System;
using System.ComponentModel.DataAnnotations;

namespace DrankReus_api.Data
{

  public class UserInformation
  {
        public int Id { get; set; }

        [Required(ErrorMessage = "Een email adres is verplicht")]
        public string Email { get; set; }

        public string Password { get; set; } = null;

        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string FirstName { get; set; }

        public string Prefix { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht")]
        public string LastName { get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }

        public string PostalCode { get; set; }

        public string Area { get; set; }
        
        public int? DiscountPoints { get; set; }

        public bool? Admin { get; set; }
  }
    
}