using CarRepairWorkshop.Shared;

namespace CarRepairWorkshop.API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(Guid customerId);
        Task<Customer> GetCustomerByNameAsync(string customerName);
        Task AddCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid customerId);
        Task UpdateCustomerDataAsync(Customer customer);
    }
}
