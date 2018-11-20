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

        public Brand BrandEntity { get; set; }
        public Category CategoryEntity { get; set; }
        public Country CountryEntity { get; set; }
        public int Inventory { get;set;}
    }
}