using PDP.University.Examine.Project.Web.API.DTOs.ProductDtos;

namespace PDP.University.Examine.Project.Web.API.DTOs.CategoryDTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        //Navigation property for related products
        public ICollection<ProductDto> Products { get; set; }
    }
}
