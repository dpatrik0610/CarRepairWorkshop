using CarRepairWorkshop.API.Services;
using CarRepairWorkshop.Shared.Enums;
using CarRepairWorkshop.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

public class WorkOrderServiceUnitTests
{
    private WorkshopDbContext GenerateTestContext()
    {
        var options = new DbContextOptionsBuilder<WorkshopDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        return new WorkshopDbContext(options);
    }

    [Fact]
    public async Task GetWorkOrdersByCustomerIdAsync_ReturnsWorkOrdersForCustomerId()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<WorkOrderService>>();
        var service = new WorkOrderService(logger, dbContext);

        var customerId = Guid.NewGuid();
        var workOrder1 = new WorkOrder { 
            CustomerId = customerId,
            LicensePlate = "XXX-123",
            RepairCategory = RepairCategory.Karosszeria,
            DateOfProduction = DateTime.Now.AddYears(-5),
            DamageSeverity = 5,
            Status = JobStatus.Recorded
        };

        var workOrder2 = new WorkOrder
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            LicensePlate = "XXX-123",
            RepairCategory = RepairCategory.Karosszeria,
            DateOfProduction = DateTime.Now.AddYears(-5),
            DamageSeverity = 5,
            Status = JobStatus.Recorded
        };
        var workOrder3 = new WorkOrder
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            LicensePlate = "XXX-123",
            RepairCategory = RepairCategory.Karosszeria,
            DateOfProduction = DateTime.Now.AddYears(-5),
            DamageSeverity = 5,
            Status = JobStatus.Recorded
        };

        dbContext.WorkOrders.AddRange(workOrder1, workOrder2, workOrder3);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetWorkOrdersByCustomerIdAsync(customerId);

        // Assert
        Assert.Single(result);
        Assert.Equal(workOrder1, result.First());
    }

    [Fact]
    public async Task GetWorkOrderByIdAsync_ReturnsCorrectWorkOrder()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<WorkOrderService>>();
        var service = new WorkOrderService(logger, dbContext);

        var workOrder1 = new WorkOrder
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            LicensePlate = "XXX-123",
            RepairCategory = RepairCategory.Karosszeria,
            DateOfProduction = DateTime.Now.AddYears(-5),
            DamageSeverity = 5,
            Status = JobStatus.Recorded
        };
        var workOrder2 = new WorkOrder
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            LicensePlate = "XXX-123",
            RepairCategory = RepairCategory.Karosszeria,
            DateOfProduction = DateTime.Now.AddYears(-5),
            DamageSeverity = 5,
            Status = JobStatus.Recorded
        };

        dbContext.WorkOrders.AddRange(workOrder1, workOrder2);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetWorkOrderByIdAsync(workOrder1.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(workOrder1, result);
    }

    [Fact]
    public async Task AddWorkOrderAsync_AddsWorkOrderToDatabase()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<WorkOrderService>>();
        var service = new WorkOrderService(logger, dbContext);

        var workOrder = new WorkOrder
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            LicensePlate = "XXX-123",
            RepairCategory = RepairCategory.Karosszeria,
            DateOfProduction = DateTime.Now.AddYears(-5),
            DamageSeverity = 5,
            Status = JobStatus.Recorded
        };
        // Act
        await service.AddWorkOrderAsync(workOrder);

        // Assert
        var result = await dbContext.WorkOrders.FirstOrDefaultAsync(w => w.Id == workOrder.Id);
        Assert.NotNull(result);
        Assert.Equal(workOrder, result);
    }

    [Fact]
    public async Task DeleteWorkOrderAsync_DeletesWorkOrderFromDatabase()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<WorkOrderService>>();
        var service = new WorkOrderService(logger, dbContext);

        var workOrder = new WorkOrder
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            LicensePlate = "XXX-123",
            RepairCategory = RepairCategory.Karosszeria,
            DateOfProduction = DateTime.Now.AddYears(-5),
            DamageSeverity = 5,
            Status = JobStatus.Recorded
        };
        dbContext.WorkOrders.Add(workOrder);
        await dbContext.SaveChangesAsync();

        // Act
        await service.DeleteWorkOrderAsync(workOrder.Id);

        // Assert
        var result = await dbContext.WorkOrders.FirstOrDefaultAsync(w => w.Id == workOrder.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateWorkOrderStatusAsync_UpdatesWorkOrderStatusInDatabase()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<WorkOrderService>>();
        var service = new WorkOrderService(logger, dbContext);

        var workOrder = new WorkOrder {
            Status = JobStatus.Recorded,
            LicensePlate = "XXX-123",
            RepairCategory = RepairCategory.Karosszeria,
            DateOfProduction = DateTime.Now.AddYears(-5),
            DamageSeverity = 5
        };

        dbContext.WorkOrders.Add(workOrder);
        await dbContext.SaveChangesAsync();

        var newStatus = JobStatus.InProgress;

        // Act
        await service.UpdateWorkOrderStatusAsync(workOrder.Id, newStatus);

        // Assert
        var result = await dbContext.WorkOrders.FirstOrDefaultAsync(w => w.Id == workOrder.Id);
        Assert.NotNull(result);
        Assert.Equal(newStatus, result.Status);
    }


    /*[Theory]
    [InlineData(RepairCategory.Karosszeria, -5, 5, JobStatus.Recorded, "XXX-123")] // Invalid DateOfProduction
    [InlineData(RepairCategory.Motor, 10, 0, JobStatus.InProgress, "XXX-123")] // Invalid damage
    [InlineData(RepairCategory.Motor, 10, 1, JobStatus.InProgress, "X2X-123")] // Invalid licensePlate
    public async Task AddWorkOrderAsync_InvalidInput_ThrowsException(
        RepairCategory repairCategory, int carAge, int damageSeverity, JobStatus status, string licensePlate)
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<WorkOrderService>>();
        var service = new WorkOrderService(logger, dbContext);

        var invalidWorkOrder = new WorkOrder
        {
            CustomerId = Guid.NewGuid(),
            RepairCategory = repairCategory,
            DateOfProduction = DateTime.Now.AddYears(carAge),
            DamageSeverity = damageSeverity,
            Status = status,
            LicensePlate = licensePlate
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.AddWorkOrderAsync(invalidWorkOrder));
    }*/
}
