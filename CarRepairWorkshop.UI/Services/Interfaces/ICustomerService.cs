using CarRepairWorkshop.Shared;

namespace CarRepairWorkshop.UI.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>?> GetCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task<Customer?> GetCustomerByNameAsync(string customerName);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);
    }
}
