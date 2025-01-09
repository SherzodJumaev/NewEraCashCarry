namespace PDP.University.Examine.Project.Web.API.DTOs.OrderDTOs
{
    public class UpdateOrderRequestDto
    {
        public int CustomerId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }
    }
}
