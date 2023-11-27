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
    public async Task GetAllWorkOrdersAsync_ReturnsAllWorkOrders()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<WorkOrderService>>();
        var service = new WorkOrderService(logger, dbContext);
        int numberOfCustomers = 10;
        // Add work orders to the database
        for (int i = 0; i < numberOfCustomers; i++)
        {
            var workOrder = new WorkOrder {CustomerId = Guid.NewGuid() };
            dbContext.WorkOrders.Add(workOrder);
        }
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetAllWorkOrdersAsync();

        // Assert
        Assert.Equal(numberOfCustomers, result.Count());
    }

    [Fact]
    public async Task GetWorkOrdersByCustomerIdAsync_ReturnsWorkOrdersForCustomerId()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<WorkOrderService>>();
        var service = new WorkOrderService(logger, dbContext);

        var customerId = Guid.NewGuid();
        var workOrder1 = new WorkOrder { CustomerId = customerId };
        var workOrder2 = new WorkOrder { CustomerId = Guid.NewGuid() };

        dbContext.WorkOrders.AddRange(workOrder1, workOrder2);
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
            CustomerId = Guid.NewGuid(),
            RepairCategory = RepairCategory.Karosszeria,
            DateOfProduction = DateTime.Now.AddYears(-5),
            DamageSeverity = 5,
            Status = JobStatus.Recorded
        };

        var workOrder2 = new WorkOrder
        {
            CustomerId = Guid.NewGuid(),
            RepairCategory = RepairCategory.Motor,
            DateOfProduction = DateTime.Now.AddYears(-10),
            DamageSeverity = 3,
            Status = JobStatus.InProgress
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
            CustomerId = Guid.NewGuid(),
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

        var workOrder = new WorkOrder { };
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

        var workOrder = new WorkOrder {Status = JobStatus.Recorded };
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
}
