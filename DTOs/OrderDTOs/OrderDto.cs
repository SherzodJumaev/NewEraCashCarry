namespace PDP.University.Examine.Project.Web.API.DTOs.OrderDTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderCustomerInfo OrderCustomer { get; set; }
        public List<OrderItemInfo> OrderItemsInfo { get; set; }

        public string PaymentStatus { get; set; }
    }
}
