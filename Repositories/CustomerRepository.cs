using Microsoft.EntityFrameworkCore;
using PDP.University.Examine.Project.Web.API.Data;
using PDP.University.Examine.Project.Web.API.Interfaces;
using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDBContext _context;
        public CustomerRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> DeleteAsync(int id)
        {
            var foundCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

            if (foundCustomer == null)
            {
                return null;
            }

            _context.Customers.Remove(foundCustomer);
            await _context.SaveChangesAsync();

            return foundCustomer;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var foundCustomer = await _context.Customers.FindAsync(id);

            if (foundCustomer == null)
            {
                return null;
            }

            return foundCustomer;
        }

        public async Task<Customer> UpdateAsync(int id, Customer customer)
        {
            var foundCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

            if (foundCustomer == null)
            {
                return null;
            }

            foundCustomer.FirstName = customer.FirstName;
            foundCustomer.LastName = customer.LastName;
            foundCustomer.Address = customer.Address;
            foundCustomer.PhoneNumber = customer.PhoneNumber;
            foundCustomer.Email = customer.Email;

            await _context.SaveChangesAsync();

            return foundCustomer;
        }

        public async Task<bool> CustomerExists(int id)
        {
            if (id <= 0)
            {
                return false;   // Basic validation to ensure valid id
            }

            return await _context.Customers.AnyAsync(e => e.Id == id);
        }

        public async Task<long> LengthAsync()
        {
            var length = await _context.Customers.MaxAsync(c => c.Id);

            return length;
        }
    }
}
