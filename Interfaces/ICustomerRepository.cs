using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer> UpdateAsync(int id, Customer customer);
        Task<Customer> DeleteAsync(int id);
        Task<long> LengthAsync();
        Task<bool> CustomerExists(int id);
    }
}
