namespace PDP.University.Examine.Project.Web.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        // [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
    }
}
