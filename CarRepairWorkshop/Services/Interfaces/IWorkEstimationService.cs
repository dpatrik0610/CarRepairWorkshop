using CarRepairWorkshop.Shared.Enums;

namespace CarRepairWorkshop.API.Services.Interfaces
{
    public interface IWorkEstimationService
    {
        double getWeightMultiplierByAge(int age);
        double getWeightMultiplierByDamage(int damageSeverity);
        int getCarAge(DateTime productionDate);
        double CalculateWorkHourEstimation(RepairCategory category, double weightByAge, double weightByDamage);
    }
}
