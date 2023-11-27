using CarRepairWorkshop.UI;
using CarRepairWorkshop.UI.Services;
using CarRepairWorkshop.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") });
builder.Services.AddScoped <ICustomerService, CustomerService>();
builder.Services.AddScoped <IWorkOrderService, WorkOrderService>();
await builder.Build().RunAsync();
