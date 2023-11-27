using CarRepairWorkshop.API.Services.Interfaces;
using CarRepairWorkshop.API.Services;
using CarRepairWorkshop.Shared.Enums;
using CarRepairWorkshop.Shared;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Runtime.CompilerServices;

public class WorkEstimationServiceUnitTests
{
    private readonly WorkEstimationService _sut;
    private readonly ILogger<WorkEstimationService> _logger;
    public WorkEstimationServiceUnitTests()
    {
        _logger = Substitute.For<ILogger<WorkEstimationService>>();
        _sut = new WorkEstimationService(_logger);
    }

    [Theory]
    [InlineData(RepairCategory.Karosszeria, 5, 5, 1.8)] // Example: Category: Karosszeria, 5 years old, damage severity 5
    [InlineData(RepairCategory.Motor, 10, 3, 4.8)] // Example: Category: Motor, 10 years old, damage severity 3
    [InlineData(RepairCategory.Futomu, 15, 8, 7.2)] // Example: Category: Futomu, 15 years old, damage severity 8
    public void CalculateWorkHourEstimation_ValidWorkOrder_ReturnsCorrectEstimation(
        RepairCategory repairCategory, int carAge, int damageSeverity, double expectedEstimation)
    {
        // Arrange
        var workOrder = new WorkOrder
        {
            RepairCategory = repairCategory,
            DateOfProduction = DateTime.Now.AddYears(-carAge),
            DamageSeverity = damageSeverity
        };

        // Act
        var result = _sut.CalculateWorkHourEstimation(workOrder);

        // Assert
        Assert.Equal(expectedEstimation, Math.Round(result, 2));
    }

    [Fact]
    public void CalculateWorkHourEstimation_NullWorkOrder_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _sut.CalculateWorkHourEstimation(null));
    }

    [Fact]
    public void CalculateWorkHourEstimation_FutureProductionDate_ThrowsArgumentException()
    {
        // Arrange
        var workOrder = new WorkOrder
        {
            DateOfProduction = DateTime.Now.AddYears(1)
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _sut.CalculateWorkHourEstimation(workOrder));
        Assert.Contains("production date cannot be in the future", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}
