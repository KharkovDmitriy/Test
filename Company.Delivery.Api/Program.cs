using Company.Delivery.Api.AppStart;
using Company.Delivery.Database;
using Company.Delivery.Domain;
using Company.Delivery.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDeliveryControllers();
builder.Services.AddDeliveryApi();

string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("ConnectionString is not found");
builder.Services.AddDbContext<DeliveryDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IWaybillService, WaybillService>();

var app = builder.Build();

app.UseDeliveryApi();
app.MapControllers();

app.Run();