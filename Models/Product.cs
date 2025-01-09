using System.ComponentModel.DataAnnotations.Schema;

namespace PDP.University.Examine.Project.Web.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string ImageUrl { get; set; }
    }
}
