using CarRepairWorkshop.Shared;
using CarRepairWorkshop.Shared.Enums;

namespace CarRepairWorkshop.UI.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<IEnumerable<WorkOrder>?> GetWorkOrdersAsync();
        Task<IEnumerable<WorkOrder>?> GetWorkOrdersByCustomerIdAsync(Guid workOrderName);
        Task<WorkOrder?> GetWorkOrderByIdAsync(Guid workOrderId);
        Task AddWorkOrderAsync(WorkOrder workOrder);
        Task UpdateWorkOrderStatusAsync(Guid id);
        Task DeleteWorkOrderAsync(Guid workOrderId);
        Task<double?> CalculateEstimatedTime(Guid id);
    }
}
