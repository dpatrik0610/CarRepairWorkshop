using CarRepairWorkshop.Shared;
using Microsoft.EntityFrameworkCore;

public class WorkshopDbContext : DbContext
{
    public WorkshopDbContext(DbContextOptions<WorkshopDbContext> options): base(options){ }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .ToTable("Customer")
            .HasKey(c => c.Id);

        modelBuilder.Entity<WorkOrder>()
            .ToTable("WorkOrder")
            .HasKey(c => c.Id);
    }
}