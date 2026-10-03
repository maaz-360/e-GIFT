using eGift.WebAPI.Common;
using eGift.WebAPI.Data;
using eGift.WebAPI.Helpers;
using eGift.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Middlewares;

public class DefaultEmployeeMiddleware
{
    private readonly RequestDelegate _next;

    public DefaultEmployeeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, AppDBContext context)
    {
        // Check if default employee already exists
        var defaultEmployee = await context.Employees
            .FirstOrDefaultAsync(x => x.IsDefault);

        if (defaultEmployee == null)
        {
            // Create default employee
            var employee = new EmployeeModel
            {
                FirstName = "Mohammad",
                LastName = "Maaz",
                DateofBirth = new DateTime(2003, 12, 19),
                GenderId = 1,
                Mobile = "8177915624",
                Email = "maaz.hbc@gmail.com",
                AddressId = 1,
                IsActive = true,
                ProfileImagePath = null,
                ProfileImageData = null,
                RoleId = 1,
                IsDefault = true,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            context.Employees.Add(employee);

            await context.SaveChangesAsync();

            string hashedPassword = PasswordHelper.HashPassword("Admin@123");

            // Create login for the employee
            var login = new LoginModel
            {
                RefId = employee.Id,
                RefType = RefType.Employee.ToString(),
                UserName = "Maaz",
                Password = hashedPassword,
                RoleId = employee.RoleId,
                IsActive = true,
                LogInDate = null,
                LastLoginDate = null,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            context.Logins.Add(login);

            await context.SaveChangesAsync();
        }

        // Continue the request pipeline
        await _next(httpContext);
    }
}