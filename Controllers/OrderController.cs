using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDP.University.Examine.Project.Web.API.Data;
using PDP.University.Examine.Project.Web.API.DTOs.OrderDTOs;
using PDP.University.Examine.Project.Web.API.Interfaces;
using PDP.University.Examine.Project.Web.API.Mappers.OrderMaps;
using PDP.University.Examine.Project.Web.API.Models;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PDP.University.Examine.Project.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _ordersRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderController(
            IOrderRepository ordersRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository)
        {
            _ordersRepository = ordersRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>A list of orders</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var orders = await _ordersRepository.GetAllAsync();

                Log.Information($"Orders fetched successfully.");

                return Ok(orders.Select(o => o.FromOrderToOrderDto()));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to fetch Orders");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Retrieves a specific order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order with the given ID or an error message if not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _ordersRepository.GetByIdAsync(id);

                if (id <= 0)
                {
                    return BadRequest();
                }
                else if (order == null)
                {
                    return NotFound($"Order with the given id:{id} is not found.");
                }

                Log.Information($"Order with the given ID: {id} fetched successfully.");

                return Ok(order.FromOrderToOrderDto());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to fetch Order.");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderDto">The order details to create the order.</param>
        /// <returns>The created order with a location header.</returns>
        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _customerRepository.GetByIdAsync(orderDto.CustomerId);

            if (customer == null)
            {
                return BadRequest("Customer not found.");
            }

            try
            {
                var order = new Order
                {
                    CustomerId = orderDto.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>()
                };

                decimal totalAmount = 0;
                foreach (var itemDto in orderDto.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                    if (product == null || product.Stock < itemDto.Quantity)
                    {
                        return BadRequest($"Insufficient stock for product ID {itemDto.ProductId}");
                    }

                    var orderItem = new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        Price = product.Price * itemDto.Quantity,
                        Product = product // This will be set but not needed at this point for the POST operation
                    };

                    order.OrderItems.Add(orderItem);
                    totalAmount += orderItem.Price;
                    product.Stock -= itemDto.Quantity;
                }

                order.TotalAmount = totalAmount;

                var createdOrder = await _ordersRepository.CreateAsync(order);

                Log.Information($"Order {createdOrder} created with ID {createdOrder.Id}", createdOrder.CustomerId, createdOrder.Customer);

                return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder.FromOrderToOrderDto());
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to create Order {orderDto.CustomerId}");
                throw; 
            }
        }

        /// <summary>
        /// Deletes an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>No content if successful or an error message if not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _ordersRepository.DeleteAsync(id);

                if (id <= 0)
                {
                    return BadRequest();
                }
                else if (order == null)
                {
                    return NotFound($"Order with the given id:{id} is not found.");
                }

                Log.Information($"Order {order} deleted successfully with ID {order.Id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete order");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Retrieves the status of an order by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve the status for.</param>
        /// <returns>
        /// Returns 200 (OK) with the order status if found,  
        /// 400 (Bad Request) if the ID is invalid,  
        /// or 404 (Not Found) if no order exists with the given ID.
        /// </returns>
        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _ordersRepository.GetByIdAsync(id);

                if (id <= 0)
                {
                    return BadRequest();
                }
                else if (order == null)
                {
                    return NotFound($"Order Status with the given id:{id} is not found.");
                }

                Log.Information($"Order Status {order} gotten with ID {order.Id}.");

                return Ok(order.ToOrderStatus());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get order status");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Retrieves a list of orders for a given customer by their unique identifier.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve orders for.</param>
        /// <returns>
        /// Returns 200 (OK) with the list of orders if found,  
        /// 400 (Bad Request) if the customer ID is invalid,  
        /// or 404 (Not Found) if no orders exist for the given customer ID.
        /// </returns>
        [HttpGet("customerId/{customerId}")]
        public async Task<IActionResult> GetByCustomerId([FromRoute] int customerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var orders = await _ordersRepository.GetByCustomerIdAsync(customerId);

                if (customerId <= 0)
                {
                    return BadRequest();
                }
                else if (orders is null)
                {
                    return NotFound($"Orders with the given customer id:{customerId} is not found.");
                }

                Log.Information($"Orders gotten with ID {customerId}");

                return Ok(orders.Select(o => o.ToOrderDtoForCustomer()));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to get orders.");
                throw; // Let the error middleware handle the exception
            }
        }
    }
}
