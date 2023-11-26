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
            RepairCategory category = workOrder.RepairCategory;
            int carAge = getCarAge(workOrder.DateOfProduction);
            double weightByAge = getWeightMultiplierByAge(carAge);
            double weightByDamage = getWeightMultiplierByDamage(workOrder.DamageSeverity);

            try
            {
                double calculateFormula = (int)category * weightByAge * weightByDamage;
                return calculateFormula;
            }
            catch   (Exception ex)
            {
                _logger.LogError($"Error at CalculateWorkHourEstimation() : {ex.Message}");
                throw;
            }
        }

        private double getWeightMultiplierByAge(int age)
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

        private int getCarAge(DateTime productionDate)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - productionDate.Year;
            // Basically if the current month and day is sooner than the day it was produced, than the "birthday" did not occure yet.
            if (currentDate.Month < productionDate.Month || (currentDate.Month == productionDate.Month && currentDate.Day < productionDate.Day))
            {
                age--;
            }

            return age;
        }

        private double getWeightMultiplierByDamage(int damageSeverity)
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
