using System.ComponentModel.DataAnnotations;

namespace DrankReus_api.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Een naam is verplicht")]
        public string Name { get; set; }
    }
}