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

    [Fact]
    public async Task GetCustomerByNameAsync_ReturnsCorrectCustomer()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<CustomerService>>();
        var service = new CustomerService(logger, dbContext);

        var customerName = "John Doe";
        var customer = new Customer
        {
            Name = customerName,
            Address = "123 Main St",
            Email = "john.doe@example.com"
        };
        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await service.GetCustomerByNameAsync(customerName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerName, result.Name);
    }

    [Fact]
    public async Task AddCustomerAsync_AddsCustomerToDatabase()
    {
        // Arrange
        WorkshopDbContext dbContext = GenerateTestContext();
        var logger = Substitute.For<ILogger<CustomerService>>();
        var service = new CustomerService(logger, dbContext);

        var customer = new Customer
        {
            Name = "John Doe",
            Address = "123 Main St",
            Email = "john.doe@example.com"
        };

        // Act
        await service.AddCustomerAsync(customer);

        // Assert
        var result = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
        Assert.NotNull(result);
        Assert.Equal(customer.Name, result.Name);
    }

    [Fact]
    public async Task DeleteCustomerAsync_DeletesCustomerFromDatabase()
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
        await service.DeleteCustomerAsync(customerId);

        // Assert
        var result = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateCustomerDataAsync_UpdatesCustomerInDatabase()
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

        var updatedCustomerData = new Customer
        {
            Id = customerId,
            Name = "Updated Name",
            Address = "456 New St",
            Email = "updated.email@example.com"
        };

        // Act
        await service.UpdateCustomerDataAsync(updatedCustomerData);

        // Assert
        var result = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        Assert.NotNull(result);
        Assert.Equal(updatedCustomerData.Name, result.Name);
        Assert.Equal(updatedCustomerData.Address, result.Address);
        Assert.Equal(updatedCustomerData.Email, result.Email);
    }
}
