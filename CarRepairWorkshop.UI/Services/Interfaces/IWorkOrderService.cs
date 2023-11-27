using CarRepairWorkshop.Shared;

namespace CarRepairWorkshop.UI.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<IEnumerable<WorkOrder>?> GetWorkOrdersAsync();
        Task<IEnumerable<WorkOrder>?> GetWorkOrdersByCustomerIdAsync(string workOrderName);
        Task<WorkOrder?> GetWorkOrderByIdAsync(int workOrderId);
        Task AddWorkOrderAsync(WorkOrder workOrder);
        Task UpdateWorkOrderAsync(WorkOrder workOrder);
        Task DeleteWorkOrderAsync(int workOrderId);
    }
}
