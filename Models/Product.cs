using System.ComponentModel.DataAnnotations;

namespace DrankReus_api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? CountryId { get; set; }
        public int? BrandId { get; set; }
        
        [Required(ErrorMessage = "Geen product naam ingevoerd")]
        public string Name{ get; set; }
        
        [Required(ErrorMessage = "Geen product omschrijving ingevoerd")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Geen product prijs ingevoerd")]
        public decimal Price{ get; set; }
        
        [Required(ErrorMessage = "Geen product aantal ingevoerd")]
        public string Volume { get; set; }
        
        [Required(ErrorMessage = "Geen alcoholprecentage ingevoerd")]
        public decimal Alcoholpercentage { get; set; }
        
        [Required(ErrorMessage = "Geen product afbeelding ingevoerd")]
        public string Url { get; set; }

        public Brand BrandEntity { get; set; }
        public Category CategoryEntity { get; set; }
        public Country CountryEntity { get; set; }
        public int Inventory { get;set;}
        public bool Removed { get; set; } = false;
    }
}