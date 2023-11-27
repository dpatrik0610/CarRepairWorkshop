using CarRepairWorkshop.Shared;

namespace CarRepairWorkshop.UI.Services.Interfaces
{
    public interface ICalculateEstimatedTimeService
    {
        double CalculateEstimatedTime(WorkOrder workOrder);
    }
}
