using System.ComponentModel.DataAnnotations;

namespace PDP.University.Examine.Project.Web.API.DTOs.CategoryDTOs
{
    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
