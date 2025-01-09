using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDP.University.Examine.Project.Web.API.DTOs.CustomerDTOs;
using PDP.University.Examine.Project.Web.API.Interfaces;
using PDP.University.Examine.Project.Web.API.Mappers.CustomerMaps;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PDP.University.Examine.Project.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        // GET: api/<CustomerController>
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>A list of customers</returns>
        [HttpGet]
        [Authorize("Admin")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var customers = await _customerRepository.GetAllAsync();

                Log.Information($"Customers fetched successfully.");

                return Ok(customers);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to fetch Customers.");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Retrieves a customer by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A customer with the given ID</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (id <= 0)
                {
                    return BadRequest();
                }
                else if (customer == null)
                {
                    return NotFound($"Customer with the given id:{id} is not found.");
                }

                Log.Information($"Customer with the given ID: {id} fetched successfully.");

                return Ok(customer.ToCustomerFromCustomerDto());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to fetch Customer");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns>The created customer</returns>
        [HttpPost]
        [Authorize("Admin")]
        public async Task<IActionResult> Create([FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var customerModel = customerDto.ToCustomerFromCreate();
                var createdCustomer = await _customerRepository.CreateAsync(customerModel);

                Log.Information($"Customer {createdCustomer} created with ID {createdCustomer.Id}", createdCustomer.FirstName, createdCustomer.LastName);

                return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to create Customer {customerDto.FirstName} {customerDto.LastName}");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerDto"></param>
        /// <returns>The updated customer</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id <= 0)
            {
                return BadRequest();
            }
            else if (id > await _customerRepository.LengthAsync())
            {
                NotFound($"Customer with the given id:{id} is not found.");
            }

            try
            {
                var customerModel = customerDto.ToCustomerFromCreate();
                var updatedCustomer = await _customerRepository.UpdateAsync(id, customerModel);

                Log.Information($"Customer {updatedCustomer.FirstName} updated successfully with ID {updatedCustomer.Id}");

                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update Customer {customerDto.FirstName}", customerDto.LastName);
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Deletes a customer by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>No content if deletion is successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var length = await _customerRepository.LengthAsync();
                var deletedCustomer = await _customerRepository.DeleteAsync(id);

                if (id <= 0)
                {
                    return BadRequest();
                }
                else if (id > length || deletedCustomer == null)
                {
                    return NotFound();
                }

                Log.Information($"Customer {deletedCustomer.FirstName} deleted with ID {deletedCustomer.Id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to delete customer.");
                throw; // Let the error middleware handle the exception
            }
        }
    }
}
