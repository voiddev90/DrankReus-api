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

        public void HashPassword()
        {
            this.Password = this.EncryptPassword(this.Password);
        }

        private string EncryptPassword(string password)
        {
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }

        public bool PasswordMatch(string password)
        {
            if(this.Password == this.EncryptPassword(password)) return true;
            return false;
        }

        public object UserData()
        {
            return new {
                    Id = this.Id,
                    Email = this.Email,
                    FirstName = this.FirstName,
                    Prefix = this.Prefix,
                    LastName = this.LastName,
                    Steet = this.Street,
                    BuildingNumber = this.BuildingNumber,
                    PostalCode = this.PostalCode,
                    Area = this.Area,
                    Admin = this.Admin
                };
        }
    }
}
