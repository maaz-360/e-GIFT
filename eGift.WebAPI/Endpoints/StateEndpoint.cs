using System.Text.RegularExpressions;
using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class StateEndpoint
{
    public static RouteGroupBuilder MapStateEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/state").WithTags("State");
        #region Default CRUD Endpoints
        // GET: api/state
        group.MapGet("/", async (AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("StateEndpoint");
            try
            {
                var states = await (
                    from state in context.States
                    join country in context.Countries on state.CountryId equals country.Id
                    where !state.IsDeleted
                    select new
                    {
                        Id = state.Id,
                        StateCode = state.StateCode,
                        StateName = state.StateName,
                        CountryId = state.CountryId,
                        CountryName = country.CountryName,
                        Description = state.Description,
                        CreatedDate = state.CreatedDate
                    }
                ).AsNoTracking()
                  .ToListAsync();

                return states is null ? Results.NotFound() : Results.Ok(states);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in StateEndpoint: /api/state GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving states.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/state/{id}
        group.MapGet("/{id:int}", async (
            int id,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("StateEndpoint");

            try
            {
                var state = await (
                    from s in context.States
                    join country in context.Countries
                        on s.CountryId equals country.Id
                    where s.Id == id && !s.IsDeleted
                    select new
                    {
                        Id = s.Id,
                        StateCode = s.StateCode,
                        StateName = s.StateName,
                        CountryId = s.CountryId,
                        CountryName = country.CountryName,
                        Description = s.Description,
                        CreatedDate = s.CreatedDate
                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

                return state is null
                    ? Results.NotFound()
                    : Results.Ok(state);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in StateEndpoint: /api/state/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the state with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/state
        group.MapPost("/", async (
            StateDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("StateEndpoint");

            try
            {
                var state = dto.ToEntity();

                context.States.Add(state);
                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/state/{state.Id}",
                    state
                );
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in StateEndpoint: /api/state POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the state.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // PUT: api/state/{id}
        group.MapPut("/{id:int}", async (
            int id,
            EditStateDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("StateEndpoint");

            try
            {
                var existingState = await context.States.FindAsync(id);

                if (existingState is null)
                {
                    return Results.NotFound();
                }

                existingState.ToEntity(dto);

                context.States.Update(existingState);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in StateEndpoint: /api/state/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the state with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // DELETE: api/state/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (
            int id,
            int loginUserId,
            DateTime deletedDate,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("StateEndpoint");

            try
            {
                var existingState = await context.States.FindAsync(id);

                if (existingState is null)
                {
                    return Results.NotFound();
                }

                existingState.IsDeleted = true;
                existingState.UpdatedBy = loginUserId;
                existingState.UpdatedDate = deletedDate;

                context.States.Update(existingState);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in StateEndpoint: /api/state/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the state with ID {id}.",
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