using Microsoft.EntityFrameworkCore;
using PDP.University.Examine.Project.Web.API.Data;
using PDP.University.Examine.Project.Web.API.Interfaces;
using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        public OrderRepository(
            ApplicationDBContext context,
            IProductRepository productRepository,
            ICustomerRepository customerRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var createdOrder = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            return createdOrder!;
        }

        public async Task<Order> DeleteAsync(int id)
        {
            var foundOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (foundOrder == null)
            {
                return null!;
            }

            foreach (var item in foundOrder.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                }
            }

            _context.Orders.Remove(foundOrder);
            await _context.SaveChangesAsync();

            return foundOrder;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
        {
            var orders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .ToListAsync();

            if (orders == null)
            {
                return null;
            }

            return orders;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var foundOrder = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (foundOrder == null)
            {
                return null!;
            }

            return foundOrder;
        }

        public async Task<bool> OrderExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }
    }
}
