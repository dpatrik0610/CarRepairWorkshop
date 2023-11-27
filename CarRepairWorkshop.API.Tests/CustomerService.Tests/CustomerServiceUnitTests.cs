using CarRepairWorkshop.API.Services;
using CarRepairWorkshop.Shared;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
public class CustomerServiceUnitTests
{
    private WorkshopDbContext GenerateTestContext()
    {
        var options = new DbContextOptionsBuilder<WorkshopDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        return new WorkshopDbContext(options);
    }

    [Theory]
    [InlineData(1, "John Doe", "123 Main St", "john.doe@example.com")]
    [InlineData(2, "Alice Smith", "456 Oak St", "alice.smith@example.com")]
    public async Task GetAllCustomersAsync_ReturnsAllCustomers(int numberOfCustomers, string name, string address, string email)
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();

        var logger = Substitute.For<ILogger<CustomerService>>();
        var service = new CustomerService(logger, dbContext);

        // Add customers to the database
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = $"{name}",
            Address = $"{address}",
            Email = $"{email}"
        };
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetAllCustomersAsync();

        // Assert
        Assert.Equal(numberOfCustomers, result.Count());
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ReturnsCorrectCustomer()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<CustomerService>>();
        var service = new CustomerService(logger, dbContext);

        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId,
            Name = "John Doe",
            Address = "123 Main St",
            Email = "john.doe@example.com"
        };
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetCustomerByIdAsync(customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.Id);
    }
}
