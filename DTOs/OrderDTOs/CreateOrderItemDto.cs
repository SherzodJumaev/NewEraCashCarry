using NewEraCashCarry.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PDP.University.Examine.Project.Web.API.DTOs.OrderDTOs
{
    public class CreateOrderItemDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be at least 1.")]
        public int Quantity { get; set; }
    }
}
