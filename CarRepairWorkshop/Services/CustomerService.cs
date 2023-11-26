using CarRepairWorkshop.API.Services.Interfaces;
using CarRepairWorkshop.Shared;
using Microsoft.EntityFrameworkCore;

namespace CarRepairWorkshop.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly WorkshopDbContext _dbContext;

        public CustomerService(ILogger<CustomerService> logger, WorkshopDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _dbContext.Customers.ToListAsync();
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllCustomersAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
        {
            try
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == customerId);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCustomerByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Customer> GetCustomerByNameAsync(string customerName)
        {
            try
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Name == customerName);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCustomerByNameAsync: {ex.Message}");
                throw;
            }
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            try
            {
                _dbContext.Customers.Add(customer);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddCustomerAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteCustomerAsync(Guid customerId)
        {
            try
            {
                var customerToDelete = await _dbContext.Customers.FindAsync(customerId);
                if (customerToDelete != null)
                {
                    _dbContext.Customers.Remove(customerToDelete);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteCustomerAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateCustomerDataAsync(Customer customer)
        {
            try
            {
                _dbContext.Customers.Update(customer);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateCustomerDataAsync: {ex.Message}");
                throw;
            }
        }
    }
}
