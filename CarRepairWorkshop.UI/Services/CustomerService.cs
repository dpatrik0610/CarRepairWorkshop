using CarRepairWorkshop.Shared;
using CarRepairWorkshop.UI.Services.Interfaces;
using System.Net.Http.Json;

namespace CarRepairWorkshop.UI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;
        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task<IEnumerable<Customer>?> GetCustomersAsync() =>
            _httpClient.GetFromJsonAsync<IEnumerable<Customer>>("Customer");

        public Task<Customer?> GetCustomerByIdAsync(Guid customerId) =>
            _httpClient.GetFromJsonAsync<Customer>($"Customer/{customerId}");

        public Task AddCustomerAsync(Customer customer) =>
            _httpClient.PostAsJsonAsync("Customer", customer);

        public Task DeleteCustomerAsync(Guid customerId) =>
            _httpClient.DeleteAsync($"Customer/{customerId}");

        public Task UpdateCustomerAsync(Customer newCustomerData) =>
            _httpClient.PutAsJsonAsync("Customer", newCustomerData);
    }
}
