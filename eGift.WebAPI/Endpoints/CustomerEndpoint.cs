using eGift.WebAPI.Common;
using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class CustomerEndpoint
{
    public static RouteGroupBuilder MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/customer").WithTags("Customer");

        #region Default CRUD Endpoints

        // GET: api/customer
        group.MapGet("/", async (AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CustomerEndpoint");

            try
            {
                var customers = await (
                    from customer in context.Customers
                    join gender in context.Genders on customer.GenderId equals gender.Id
                    join role in context.Roles on customer.RoleId equals role.Id
                    join address in context.Addresses on customer.AddressId equals address.Id into addressGroup
                    from address in addressGroup.DefaultIfEmpty()
                    join city in context.Cities on address.CityId equals city.Id into cityGroup
                    from city in cityGroup.DefaultIfEmpty()
                    join state in context.States on address.StateId equals state.Id into stateGroup
                    from state in stateGroup.DefaultIfEmpty()
                    join country in context.Countries on address.CountryId equals country.Id into countryGroup
                    from country in countryGroup.DefaultIfEmpty()
                    where !customer.IsDeleted
                    select new
                    {
                        Id = customer.Id,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        DateofBirth = customer.DateofBirth,
                        Age = CalculateAgeInYears(customer.DateofBirth),
                        GenderName = gender.GenderName,
                        Mobile = customer.Mobile,
                        Email = customer.Email,
                        IsActive = customer.IsActive,
                        RoleName = role.RoleName,
                        IsDefault = customer.IsDefault,
                        CreatedDate = customer.CreatedDate
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return customers is null ? Results.NotFound() : Results.Ok(customers);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CustomerEndpoint: /api/customer GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving customers.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/customer/{id}
        group.MapGet("/{id:int}", async (int id, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CustomerEndpoint");

            try
            {
                var customer = await (
           from c in context.Customers
           join gender in context.Genders on c.GenderId equals gender.Id
           join role in context.Roles on c.RoleId equals role.Id
           join address in context.Addresses on c.AddressId equals address.Id into addressGroup
           from address in addressGroup.DefaultIfEmpty()
           join city in context.Cities on address.CityId equals city.Id into cityGroup
           from city in cityGroup.DefaultIfEmpty()
           join state in context.States on address.StateId equals state.Id into stateGroup
           from state in stateGroup.DefaultIfEmpty()
           join country in context.Countries on address.CountryId equals country.Id into countryGroup
           from country in countryGroup.DefaultIfEmpty()
           join login in context.Logins on c.Id equals login.RefId
           where c.Id == id && !c.IsDeleted && login.RefType == RefType.Customer.ToString()
           select new
           {
               Id = c.Id,
               FirstName = c.FirstName,
               LastName = c.LastName,
               DateofBirth = c.DateofBirth,
               Age = CalculateAgeInYears(c.DateofBirth),

               GenderId = c.GenderId,
               GenderName = gender.GenderName,

               Mobile = c.Mobile,
               Email = c.Email,

               AddressId = c.AddressId,
               FullAddress = address == null ? null : address.Street1 + ", " + city.CityName + ", " + state.StateName + ", " + country.CountryName + " - " + address.PinCode,

               IsActive = c.IsActive,
               ProfileImagePath = c.ProfileImagePath,
               ProfileImageData = c.ProfileImageData,

               RoleId = c.RoleId,
               RoleName = role.RoleName,

               IsDefault = c.IsDefault,
               CreatedDate = c.CreatedDate,
               UserName = login.UserName,
               LastLogin = login.LastLoginDate,
               LoginId = login.Id
           }
       )
       .AsNoTracking()
       .FirstOrDefaultAsync();

                return customer is null ? Results.NotFound() : Results.Ok(customer);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CustomerEndpoint: /api/customer/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the customer with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/customer
        group.MapPost("/", async (
            [FromForm] CustomerDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment environment) =>

        {
            var logger = loggerFactory.CreateLogger("CustomerEndpoint");

            try
            {
                var customer = dto.ToEntity();

                // Create upload folder
                var uploadFolder = Path.Combine(
                    environment.ContentRootPath,
                    "uploads",
                    "customers");

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
                    customer.ProfileImagePath =
                        $"/uploads/customers/{fileName}";
                }

                context.Customers.Add(customer);
                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/customer/{customer.Id}", customer
                );
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CustomerEndpoint: /api/customer POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the customer.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }).DisableAntiforgery();

        // PUT: api/customer/{id}
        group.MapPut("/{id:int}", async (
            int id,
            [FromForm] EditCustomerDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment environment) =>
        {
            var logger = loggerFactory.CreateLogger("CustomerEndpoint");

            try
            {
                var existingCustomer = await context.Customers.FindAsync(id);

                if (existingCustomer is null)
                {
                    return Results.NotFound();
                }

                existingCustomer.ToEntity(dto);

                // Upload new image
                if (dto.ProfileImage != null &&
                    dto.ProfileImage.Length > 0)
                {
                    var uploadFolder = Path.Combine(
                        environment.ContentRootPath,
                        "uploads",
                        "customers");

                    Directory.CreateDirectory(uploadFolder);

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

                    existingCustomer.ProfileImagePath =
                        $"/uploads/customers/{fileName}";
                }

                context.Customers.Update(existingCustomer);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CustomerEndpoint: /api/customer/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the customer with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }).DisableAntiforgery();

        // DELETE: api/customer/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (int id, int loginUserId, DateTime deletedDate, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CustomerEndpoint");

            try
            {
                var existingCustomer = await context.Customers.FindAsync(id);

                if (existingCustomer is null)
                {
                    return Results.NotFound();
                }

                existingCustomer.IsDeleted = true;
                existingCustomer.UpdatedBy = loginUserId;
                existingCustomer.UpdatedDate = deletedDate;

                context.Customers.Update(existingCustomer);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CustomerEndpoint: /api/customer/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the customer with ID {id}.",
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
                "uploads",
                "customers");

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