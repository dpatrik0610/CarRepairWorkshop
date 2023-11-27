﻿using CarRepairWorkshop.Shared;
using CarRepairWorkshop.Shared.Enums;

namespace CarRepairWorkshop.UI.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<IEnumerable<WorkOrder>?> GetWorkOrdersAsync();
        Task<IEnumerable<WorkOrder>?> GetWorkOrdersByCustomerIdAsync(string workOrderName);
        Task<WorkOrder?> GetWorkOrderByIdAsync(Guid workOrderId);
        Task AddWorkOrderAsync(WorkOrder workOrder);
        Task UpdateWorkOrderStatusAsync(Guid id, JobStatus jobStatus);
        Task DeleteWorkOrderAsync(Guid workOrderId);
    }
}
