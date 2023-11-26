namespace CarRepairWorkshop.API.Services.Interfaces
{
    public interface IWorkEstimationService
    {
        int getWightMultiplierByCategory();
        double getWeightMultiplierByAge();
        double getWeightMultiplierByDamage();
        double CalculateWorkHourEstimation(string category, int ageOfCar, int damageSeverity);
    }
}
