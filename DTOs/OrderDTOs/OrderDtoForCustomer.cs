using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.DTOs.OrderDTOs
{
    public class OrderDtoForCustomer
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public List<OrderItemInfoForCustomer> OrderItemsInfo { get; set; }
    }
}
