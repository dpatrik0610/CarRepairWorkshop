﻿using CarRepairWorkshop.Shared;
using CarRepairWorkshop.Shared.Enums;

namespace CarRepairWorkshop.API.Services.Interfaces
{
    public interface IWorkOrderService
    {
        Task<IEnumerable<WorkOrder>> GetAllWorkOrdersAsync();
        Task<WorkOrder> GetWorkOrderByIdAsync(Guid workOrderId);
        Task AddWorkOrderAsync(WorkOrder workOrder);
        Task DeleteWorkOrderAsync(Guid workOrderId);
        Task UpdateWorkOrderStatusAsync(Guid workOrderId, JobStatus newStatus);
    }
}
