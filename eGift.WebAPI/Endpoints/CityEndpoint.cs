using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;
public static class CityEndpoint
{
    public static RouteGroupBuilder MapCityEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/city").WithTags("City");

        #region Default CRUD Endpoints
        // GET: api/city
        group.MapGet("/",async (AppDBContext context , ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CityEndpoint");
            try
            {
                var cities = await (
                    from city in context.Cities
                    join state in context.States on city.StateId equals state.Id
                    join country in context.Countries on state.CountryId equals country.Id
                    where !city.IsDeleted
                    select new
                    {
                        Id = city.Id,
                        CityCode = city.CityCode,
                        CityName = city.CityName,
                        StateId = city.StateId,
                        StateName = state.StateName,
                        CountryId = state.CountryId,
                        CountryName = country.CountryName,
                        Description = city.Description,
                        CreatedDate = city.CreatedDate
                    }
                ).AsNoTracking()
                .ToListAsync();
                
                return cities is null ? Results.NotFound() : Results.Ok(cities);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError("Exception in CityEndpoint: /api/city GET: {Message}.", ex.Message);
                return Results.Json(new
                {
                    Message = "An error occurred while retrieving city.",
                    error = ex.Message
                },
                statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        // GET: api/city/{id}
        group.MapGet("/{id:int}", async (
            int id,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CityEndpoint");

            try
            {
                var city = await (
              from c in context.Cities
              join s in context.States
                  on c.StateId equals s.Id
              join co in context.Countries
                  on s.CountryId equals co.Id
              where c.Id == id && !c.IsDeleted
              select new
              {
                  Id = c.Id,
                  CityCode = c.CityCode,
                  CityName = c.CityName,

                  StateId = c.StateId,
                  StateName = s.StateName,

                  CountryId = s.CountryId,
                  CountryName = co.CountryName,

                  Description = c.Description,
                  CreatedDate = c.CreatedDate
              }
          )
          .AsNoTracking()
          .FirstOrDefaultAsync();

                return city is null
                    ? Results.NotFound()
                    : Results.Ok(city);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CityEndpoint: /api/city/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the city with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/city
        group.MapPost("/", async (
            CityDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CityEndpoint");

            try
            {
                var city = dto.ToEntity();

                context.Cities.Add(city);
                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/city/{city.Id}",
                    city
                );
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CityEndpoint: /api/city POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the city.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // PUT: api/city/{id}
        group.MapPut("/{id:int}", async (
            int id,
            EditCityDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CityEndpoint");

            try
            {
                var existingCity = await context.Cities.FindAsync(id);

                if (existingCity is null)
                {
                    return Results.NotFound();
                }

                existingCity.ToEntity(dto);

                context.Cities.Update(existingCity);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CityEndpoint: /api/city/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the city with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // DELETE: api/city/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (
            int id,
            int loginUserId,
            DateTime deletedDate,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CityEndpoint");

            try
            {
                var existingCity = await context.Cities.FindAsync(id);

                if (existingCity is null)
                {
                    return Results.NotFound();
                }

                existingCity.IsDeleted = true;
                existingCity.UpdatedBy = loginUserId;
                existingCity.UpdatedDate = deletedDate;

                context.Cities.Update(existingCity);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CityEndpoint: /api/city/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the city with ID {id}.",
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