using eGift.WebAPI.Common;
using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class EmployeeEndpoint
{
    public static RouteGroupBuilder MapEmployeeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/employee").WithTags("Employee");

        #region Default CRUD Endpoints

        // GET: api/employee
        group.MapGet("/", async (AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("EmployeeEndpoint");

            try
            {
                var employees = await (
                    from employee in context.Employees
                    join gender in context.Genders on employee.GenderId equals gender.Id
                    join role in context.Roles on employee.RoleId equals role.Id
                    join address in context.Addresses on employee.AddressId equals address.Id
                    join city in context.Cities on address.CityId equals city.Id
                    join state in context.States on address.StateId equals state.Id
                    join country in context.Countries on address.CountryId equals country.Id
                    where !employee.IsDeleted
                    select new
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        DateofBirth = employee.DateofBirth,
                        GenderName = gender.GenderName,
                        Mobile = employee.Mobile,
                        Email = employee.Email,
                        IsActive = employee.IsActive,
                        IsDefault = employee.IsDefault,
                        CreatedDate = employee.CreatedDate
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return employees is null
                    ? Results.NotFound()
                    : Results.Ok(employees);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in EmployeeEndpoint: /api/employee GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving employees.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/employee/{id}
        group.MapGet("/{id:int}", async (int id, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("EmployeeEndpoint");

            try
            {
                var employee = await (
                    from e in context.Employees
                    join gender in context.Genders on e.GenderId equals gender.Id
                    join role in context.Roles on e.RoleId equals role.Id
                    join address in context.Addresses on e.AddressId equals address.Id
                    join city in context.Cities on address.CityId equals city.Id
                    join state in context.States on address.StateId equals state.Id
                    join country in context.Countries on address.CountryId equals country.Id
                    join login in context.Logins on e.Id equals login.RefId
                    where e.Id == id && !e.IsDeleted && login.RefType == RefType.Employee.ToString()
                    select new
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        DateofBirth = e.DateofBirth,
                        Age = CalculateAgeInYears(e.DateofBirth),

                        GenderId = e.GenderId,
                        GenderName = gender.GenderName,

                        Mobile = e.Mobile,
                        Email = e.Email,

                        AddressId = e.AddressId,
                        FullAddress = address.Street1 + ", " + city.CityName + ", " + state.StateName + ", " + country.CountryName + " - " + address.PinCode,

                        IsActive = e.IsActive,
                        ProfileImagePath = e.ProfileImagePath,
                        ProfileImageData = e.ProfileImageData,

                        RoleId = e.RoleId,
                        RoleName = role.RoleName,

                        IsDefault = e.IsDefault,
                        CreatedDate = e.CreatedDate,
                        UserName = login.UserName,
                        LastLogin = login.LastLoginDate,
                        LoginId = login.Id
                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

                return employee is null
                    ? Results.NotFound()
                    : Results.Ok(employee);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in EmployeeEndpoint: /api/employee/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the employee with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/employee
        group.MapPost("/", async (
            [FromForm] EmployeeDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment environment) =>
        {
            var logger = loggerFactory.CreateLogger("EmployeeEndpoint");

            try
            {
                var employee = dto.ToEntity();

                // Create upload folder
                var uploadFolder = Path.Combine(
                    environment.ContentRootPath,
                    "uploads",
                    "employees");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Save image
                if (dto.ProfileImage != null &&
                    dto.ProfileImage.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.ProfileImage.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream =
                        new FileStream(
                            filePath,
                            FileMode.Create);

                    await dto.ProfileImage.CopyToAsync(stream);

                    // Store relative path in database
                    employee.ProfileImagePath =
                        $"/uploads/employees/{fileName}";
                }

                context.Employees.Add(employee);
                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/employee/{employee.Id}",
                    employee
                );
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in EmployeeEndpoint: /api/employee POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the employee.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }).DisableAntiforgery();

        // PUT: api/employee/{id}
        group.MapPut("/{id:int}", async (
            int id,
            [FromForm] EditEmployeeDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment environment) =>
        {
            var logger = loggerFactory.CreateLogger("EmployeeEndpoint");

            try
            {
                var existingEmployee = await context.Employees.FindAsync(id);

                if (existingEmployee is null)
                {
                    return Results.NotFound();
                }

                existingEmployee.ToEntity(dto);

                // Upload new image
                if (dto.ProfileImage != null &&
                    dto.ProfileImage.Length > 0)
                {
                    var uploadFolder = Path.Combine(
                        environment.ContentRootPath,
                        "uploads",
                        "employees");

                    Directory.CreateDirectory(uploadFolder);

                    var extension = Path.GetExtension(
                        dto.ProfileImage.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.ProfileImage.CopyToAsync(stream);

                    existingEmployee.ProfileImagePath =
                        $"/uploads/employees/{fileName}";
                }

                context.Employees.Update(existingEmployee);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in EmployeeEndpoint: /api/employee/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the employee with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }).DisableAntiforgery();

        // DELETE: api/employee/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (
            int id,
            int loginUserId,
            DateTime deletedDate,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("EmployeeEndpoint");

            try
            {
                var existingEmployee = await context.Employees.FindAsync(id);

                if (existingEmployee is null)
                {
                    return Results.NotFound();
                }

                existingEmployee.IsDeleted = true;
                existingEmployee.UpdatedBy = loginUserId;
                existingEmployee.UpdatedDate = deletedDate;

                context.Employees.Update(existingEmployee);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in EmployeeEndpoint: /api/employee/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the employee with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        #endregion

        #region Image Endpoints
        group.MapGet("/image/{fileName}", (
            string fileName,
            IWebHostEnvironment environment) =>
        {
            if (fileName != Path.GetFileName(fileName))
            {
                return Results.BadRequest();
            }

            var uploadFolder = Path.Combine(
                environment.ContentRootPath,
                "Uploads",
                "Employees");

            var filePath = Path.Combine(
                uploadFolder,
                fileName);

            if (!File.Exists(filePath))
            {
                return Results.NotFound();
            }

            var extension = Path.GetExtension(fileName)
                .ToLowerInvariant();

            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

            return Results.File(filePath, contentType);
        });
        #endregion

        return group;
    }
    #region Private Methods
    // To calculate age in year from date of birth
    private static int CalculateAgeInYears(DateTime dateOfBirth)
    {
        int age = DateTime.Today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > DateTime.Today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
    #endregion
}