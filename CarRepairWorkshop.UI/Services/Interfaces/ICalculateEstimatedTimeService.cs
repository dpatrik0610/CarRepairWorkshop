using CarRepairWorkshop.Shared;

namespace CarRepairWorkshop.UI.Services.Interfaces
{
    public interface ICalculateEstimatedTimeService
    {
        Task<double?> CalculateEstimatedTime(WorkOrder workOrder);
    }
}
