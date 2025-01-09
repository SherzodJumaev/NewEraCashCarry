using System.ComponentModel.DataAnnotations;

namespace PDP.University.Examine.Project.Web.API.DTOs.OrderDTOs
{
    public class CreateOrderDto
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}
