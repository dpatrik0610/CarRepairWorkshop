using CarRepairWorkshop.API.Services.Interfaces;
using CarRepairWorkshop.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CarRepairWorkshop.API.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IWorkOrderService _workOrderService;

        public CustomerController(ICustomerService customerService, IWorkOrderService workOrderService)
        {
            _customerService = customerService;
            _workOrderService = workOrderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(Guid id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);

                if (customer == null)
                    return NotFound();

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer([FromBody] Customer customer)
        {
            try
            {
                await _customerService.AddCustomerAsync(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCustomer([FromBody] Customer newCustomerData)
        {
            try
            {
                var existingCustomer = await _customerService.GetCustomerByIdAsync(newCustomerData.Id);

                if (existingCustomer == null)
                    return NotFound();

                await _customerService.UpdateCustomerDataAsync(newCustomerData);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                var existingCustomer = await _customerService.GetCustomerByIdAsync(id);

                if (existingCustomer == null)
                    return NotFound();

                // Deleting Customer's orders.
                IEnumerable<WorkOrder> orders = await _workOrderService.GetWorkOrdersByCustomerIdAsync(existingCustomer.Id);
                if (orders.Any())
                {
                    foreach (WorkOrder order in orders) await _workOrderService.DeleteWorkOrderAsync(order.Id);
                }
                await _customerService.DeleteCustomerAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}