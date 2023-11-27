using CarRepairWorkshop.API.Services;
using CarRepairWorkshop.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// Logging
builder.Host.UseSerilog(
    (_, configuration) => configuration
        .MinimumLevel.Information()
        .WriteTo.Console()
);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WorkshopDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"));
    options.UseLazyLoadingProxies();
});

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
builder.Services.AddScoped<IWorkEstimationService, WorkEstimationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5081"));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
