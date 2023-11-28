using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarRepairWorkshop.Shared;
using CarRepairWorkshop.Shared.Enums;
using CarRepairWorkshop.UI.Services.Interfaces;

namespace CarRepairWorkshop.UI.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly HttpClient _httpClient;

        public WorkOrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WorkOrder>?> GetWorkOrdersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<IEnumerable<WorkOrder>>("WorkOrder");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
                return null;
            }
        }

        public async Task<WorkOrder?> GetWorkOrderByIdAsync(Guid workOrderId) =>
            await _httpClient.GetFromJsonAsync<WorkOrder>($"WorkOrder/{workOrderId}");

        public async Task<IEnumerable<WorkOrder>?> GetWorkOrdersByCustomerIdAsync(Guid customerId) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<WorkOrder>>($"WorkOrder/ByCustomer/{customerId}");

        public async Task AddWorkOrderAsync(WorkOrder workOrder) =>
            await _httpClient.PostAsJsonAsync("WorkOrder", workOrder);

        public async Task DeleteWorkOrderAsync(Guid workOrderId) =>
            await _httpClient.DeleteAsync($"WorkOrder/{workOrderId}");

        public async Task UpdateWorkOrderStatusAsync(Guid id)
        {
            await _httpClient.PostAsJsonAsync($"WorkOrder/Promote", id);
        }

        public async Task<double?> CalculateEstimatedTime(Guid id)
        {
            var response = await _httpClient.GetAsync($"WorkOrder/calculate/{id}");
            return await response.Content.ReadFromJsonAsync<double?>();
        }
    }
}
