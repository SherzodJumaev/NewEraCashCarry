using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);
        Task<Order> CreateAsync(Order order);
        Task<Order> DeleteAsync(int id);
        Task<bool> OrderExistsAsync(int id);
    }
}
