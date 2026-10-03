using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;
public static class CountryEndpoint
{
    public static RouteGroupBuilder MapCountryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/country").WithTags("Country");

        #region Default CRUD Endpoints
        // GET: api/country
        group.MapGet("/", async(AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CountryEndpoint");
            try
            {
                var countries = await(
                    from country in context.Countries
                    where !country.IsDeleted
                    select new
                    {
                        Id = country.Id,
                        CountryCode = country.CountryCode,
                        CountryName = country.CountryName,
                        Description = country.Description,
                        CreatedDate = country.CreatedDate
                    }
                ).AsNoTracking()
                .ToListAsync();

                return countries is null ? Results.NotFound() : Results.Ok(countries);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in CountryEndpoint: /api/country GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving countries.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/country/{id}
        group.MapGet("/{id:int}", async(int id, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CountryEndpoint");
            try
            {
                var country = await (
                    from c in context.Countries
                    where c.Id == id && !c.IsDeleted
                    select new
                    {
                        Id = c.Id,
                        CountryCode = c.CountryCode,
                        CountryName = c.CountryName,
                        Description = c.Description,
                        CreatedDate = c.CreatedDate
                    }
                ).AsNoTracking()
                .FirstOrDefaultAsync();

                return country is null ? Results.NotFound() : Results.Ok(country);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in CountryEndpoint: /api/country/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the country with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST : api/country
        group.MapPost("/", async (CountryDto dto, AppDBContext context, ILoggerFactory loggerFactory ) =>
        {
            var logger = loggerFactory.CreateLogger("CountryEndpoint");
            try
            {
                var country = dto.ToEntity();

                context.Countries.Add(country);
                await context.SaveChangesAsync();

                return Results.Created($"/api/country/{country.Id}", country);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in CountryEndpoint: /api/country POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the country.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // PUT: api/country/{id}
        group.MapPut("/{id:int}", async (
            int id,
            EditCountryDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CountryEndpoint");

            try
            {
                var existingCountry = await context.Countries.FindAsync(id);

                if (existingCountry is null)
                {
                    return Results.NotFound();
                }

                existingCountry.ToEntity(dto);

                context.Countries.Update(existingCountry);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in CountryEndpoint: /api/country/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the country with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // DELETE: api/country/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (
            int id,
            int loginUserId,
            DateTime deletedDate,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CountryEndpoint");

            try
            {
                var existingCountry = await context.Countries.FindAsync(id);

                if (existingCountry is null)
                {
                    return Results.NotFound();
                }

                existingCountry.IsDeleted = true;
                existingCountry.UpdatedBy = loginUserId;
                existingCountry.UpdatedDate = deletedDate;

                context.Countries.Update(existingCountry);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in CountryEndpoint: /api/country/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the country with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        #endregion

        return group;
    }
}