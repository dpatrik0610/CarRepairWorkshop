using CarRepairWorkshop.API.Services.Interfaces;
using CarRepairWorkshop.Shared;
using CarRepairWorkshop.Shared.Enums;

namespace CarRepairWorkshop.API.Services
{
    public class WorkEstimationService : IWorkEstimationService
    {
        private readonly ILogger<WorkEstimationService> _logger;

        public WorkEstimationService(ILogger<WorkEstimationService> logger)
        {
            _logger = logger;
        }

        public double CalculateWorkHourEstimation(WorkOrder workOrder)
        {
            try
            {
                ValidateWorkOrder(workOrder);

                RepairCategory category = workOrder.RepairCategory;
                int carAge = GetCarAge(workOrder.DateOfProduction);
                double weightByAge = GetWeightMultiplierByAge(carAge);
                double weightByDamage = GetWeightMultiplierByDamage(workOrder.DamageSeverity);

                double calculateFormula = (int)category * weightByAge * weightByDamage;
                return calculateFormula;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error at CalculateWorkHourEstimation: {ex.Message}");
                throw;
            }
        }

        private void ValidateWorkOrder(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrder.DateOfProduction > DateTime.Now)
            {
                throw new ArgumentException("The production date cannot be in the future.");
            }
        }

        private double GetWeightMultiplierByAge(int age)
        {
            if (age >= 0 && age <= 4)
            {
                return 0.5;
            }
            else if (age > 4 && age <= 9)
            {
                return 1.0;
            }
            else if (age > 9 && age <= 19)
            {
                return 1.5;
            }
            else
            {
                return 2.0;
            }
        }

        private int GetCarAge(DateTime productionDate)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - productionDate.Year;

            if (currentDate.Month < productionDate.Month || (currentDate.Month == productionDate.Month && currentDate.Day < productionDate.Day))
            {
                age--;
            }

            return age;
        }

        private double GetWeightMultiplierByDamage(int damageSeverity)
        {
            if (damageSeverity >= 1 && damageSeverity <= 2)
            {
                return 0.2;
            }
            else if (damageSeverity > 2 && damageSeverity <= 4)
            {
                return 0.4;
            }
            else if (damageSeverity > 4 && damageSeverity <= 7)
            {
                return 0.6;
            }
            else if (damageSeverity > 7 && damageSeverity <= 9)
            {
                return 0.8;
            }
            else if (damageSeverity == 10)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
