using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class AddressEndpoint
{
    public static RouteGroupBuilder MapAddressEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/address").WithTags("Address");

        #region Default CRUD Endpoints
        // GET: api/addresses
        group.MapGet("/", async (AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("AddressEndpoint");
            try
            {
                var addresses = await (
                from address in context.Addresses
                join city in context.Cities on address.CityId equals city.Id
                join state in context.States on address.StateId equals state.Id
                join country in context.Countries on address.CountryId equals country.Id
                where !address.IsDeleted
                select new
                {
                    Id = address.Id,
                    Street1 = address.Street1,
                    Street2 = address.Street2,
                    CityId = address.CityId,
                    StateId = address.StateId,
                    CountryId = address.CountryId,
                    PinCode = address.PinCode,
                    CountryName = country.CountryName,
                    StateName = state.StateName,
                    CityName = city.CityName,
                    CreatedDate = address.CreatedDate,
                    FullAddress = address.Street1 + ", " + city.CityName + ", " + state.StateName + ", " + country.CountryName + "-" + address.PinCode
                }
                ).AsNoTracking()
                .ToListAsync();

                return addresses is null ? Results.NotFound() : Results.Ok(addresses);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError("Exception in AddressEndpoint: /api/addresses GET: {Message}.", ex.Message);
                return Results.Json(new
                {
                    Message = "An error occurred while retrieving addresses.",
                    error = ex.Message
                },
                statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        // GET: api/addresses/{id}
        group.MapGet("/{id:int}", async (int id, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("AddressEndpoint");
            try
            {
                var address = await (
                    from a in context.Addresses
                    join c in context.Cities on a.CityId equals c.Id
                    join s in context.States on a.StateId equals s.Id
                    join co in context.Countries on a.CountryId equals co.Id
                    where a.Id == id && !a.IsDeleted
                    select new
                    {
                        Id = a.Id,
                        Street1 = a.Street1,
                        Street2 = a.Street2,
                        CityId = a.CityId,
                        StateId = a.StateId,
                        CountryId = a.CountryId,
                        PinCode = a.PinCode,
                        CountryName = co.CountryName,
                        StateName = s.StateName,
                        CityName = c.CityName,
                        CreatedDate = a.CreatedDate,
                        FullAddress = a.Street1 + ", " + c.CityName + ", " + s.StateName + ", " + co.CountryName + "-" + a.PinCode
                    }
                ).AsNoTracking()
                .FirstOrDefaultAsync();

                return address is null ? Results.NotFound() : Results.Ok(address);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError("Exception in AddressEndpoint: /api/addresses/{id} GET: {Message}.", id, ex.Message);
                return Results.Json(new
                {
                    Message = $"An error occurred while retrieving the address with ID {id}.",
                    error = ex.Message
                },
                statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        // POST: api/addresses
        group.MapPost("/", async (AddressDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("AddressEndpoint");
            try
            {
                var address = dto.ToEntity();

                context.Addresses.Add(address);
                await context.SaveChangesAsync();

                return Results.Created($"/api/addresses/{address.Id}", address);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError("Exception in AddressEndpoint: /api/addresses POST: {Message}.", ex.Message);
                return Results.Json(new
                {
                    Message = "An error occurred while creating the address.",
                    error = ex.Message
                },
                statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        // PUT: api/addresses/{id}
        group.MapPut("/{id:int}", async (int id, EditAddressDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("AddressEndpoint");
            try
            {
                var existingAddress = await context.Addresses.FindAsync(id);
                if (existingAddress is null)
                {
                    return Results.NotFound();
                }

                existingAddress.ToEntity(dto);

                context.Addresses.Update(existingAddress);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError("Exception in AddressEndpoint: /api/addresses/{id} PUT: {Message}.", id, ex.Message);
                return Results.Json(new
                {
                    Message = $"An error occurred while updating the address with ID {id}.",
                    error = ex.Message
                },
                statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        // DELETE: api/addresses/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (int id, int loginUserId, DateTime deletedDate, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("AddressEndpoint");
            try
            {
                var existingAddress = await context.Addresses.FindAsync(id);
                if (existingAddress is null)
                {
                    return Results.NotFound();
                }
                existingAddress.IsDeleted = true;
                existingAddress.UpdatedBy = loginUserId;
                existingAddress.UpdatedDate = deletedDate;

                context.Addresses.Update(existingAddress);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError("Exception in AddressEndpoint: /api/addresses/{id} DELETE: {Message}.", id, ex.Message);
                return Results.Json(new
                {
                    Message = $"An error occurred while deleting the address with ID {id}.",
                    error = ex.Message
                },
                statusCode: StatusCodes.Status500InternalServerError);
            }
        });
        #endregion

        return group;
    }
}