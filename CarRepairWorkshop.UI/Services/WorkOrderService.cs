using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarRepairWorkshop.Shared;
using CarRepairWorkshop.Shared.Enums;
using CarRepairWorkshop.UI.Services.Interfaces;

namespace CarRepairWorkshop.UI.Services
{
    public class WorkOrderService : IWorkOrderService, ICalculateEstimatedTimeService
    {
        private readonly HttpClient _httpClient;

        public WorkOrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WorkOrder>?> GetWorkOrdersAsync() =>
            await _httpClient.GetFromJsonAsync<IEnumerable<WorkOrder>>("WorkOrder");

        public async Task<WorkOrder?> GetWorkOrderByIdAsync(Guid workOrderId) =>
            await _httpClient.GetFromJsonAsync<WorkOrder>($"WorkOrder/{workOrderId}");

        public async Task<IEnumerable<WorkOrder>?> GetWorkOrdersByCustomerIdAsync(string workOrderName) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<WorkOrder>>($"WorkOrder/ByCustomer/{workOrderName}");

        public async Task AddWorkOrderAsync(WorkOrder workOrder) =>
            await _httpClient.PostAsJsonAsync("WorkOrder", workOrder);

        public async Task DeleteWorkOrderAsync(Guid workOrderId) =>
            await _httpClient.DeleteAsync($"WorkOrder/{workOrderId}");

        public async Task UpdateWorkOrderStatusAsync(Guid id, JobStatus jobStatus)
        {
            var updateData = new { Id = id, Status = jobStatus };
            await _httpClient.PutAsJsonAsync($"WorkOrder/{id}/UpdateStatus", updateData);
        }

        public async Task<double?> CalculateEstimatedTime(WorkOrder workOrder)
        {
            var response = await _httpClient.PostAsJsonAsync("/WorkOrder/calculate", workOrder);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<double?>();
        }
    }
}
