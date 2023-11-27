using CarRepairWorkshop.Shared;

namespace CarRepairWorkshop.UI.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>?> GetCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(Guid customerId);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid customerId);
    }
}
