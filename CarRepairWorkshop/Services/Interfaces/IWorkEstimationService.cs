using CarRepairWorkshop.Shared;

namespace CarRepairWorkshop.API.Services.Interfaces
{
    public interface IWorkEstimationService
    {
        double CalculateWorkHourEstimation(WorkOrder workorder);
    }
}
