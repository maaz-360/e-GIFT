using eGift.WebAPI.Data;
using eGift.WebAPI.Endpoints;
using eGift.WebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

// Use API key middleware
app.UseMiddleware<DefaultEmployeeMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

// Map endpoints
app.MapAddressEndpoints();
app.MapCategoryEndpoints();
app.MapCityEndpoints(); 
app.MapCountryEndpoints();
app.MapCustomerEndpoints();
app.MapEmployeeEndpoints();
app.MapGenderEndpoints();
app.MapLoginEndpoints();
app.MapOrderEndpoints();
app.MapProductEndpoints();
app.MapRoleEndpoints();
app.MapStateEndpoints();
app.MapSubCategoryEndpoints();
app.MapOrderDetailsEndpoints();
app.MapMyCartEndpoints();

app.Run();


