using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDP.University.Examine.Project.Web.API.DTOs.ProductDtos
{
    public class CreateProductRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Name cannot be over 10 characters.")]
        [MinLength(4, ErrorMessage = "Name should be over 4 characters.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Description cannot be over 30 characters.")]
        [MinLength(4, ErrorMessage = "Description should be over 4 characters.")]
        public string Description { get; set; }
        [Required]
        [Range(0.1, 99999)]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 999999999)]
        public int Stock { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
