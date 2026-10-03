using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class GenderEndpoint
{
    public static RouteGroupBuilder MapGenderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/gender").WithTags("Gender");

        #region Default CRUD Endpoints

        // GET: api/gender
        group.MapGet("/", async (
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GenderEndpoint");

            try
            {
                var genders = await (
                    from gender in context.Genders
                    where !gender.IsDeleted
                    select new
                    {
                        Id = gender.Id,
                        GenderName = gender.GenderName,
                        Description = gender.Description,
                        CreatedDate = gender.CreatedDate
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return genders is null? Results.NotFound(): Results.Ok(genders);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in GenderEndpoint: /api/gender GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving genders.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/gender/{id}
        group.MapGet("/{id:int}", async (
            int id,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GenderEndpoint");

            try
            {
                var gender = await (
                    from g in context.Genders
                    where g.Id == id && !g.IsDeleted
                    select new
                    {
                        Id = g.Id,
                        GenderName = g.GenderName,
                        Description = g.Description,
                        CreatedDate = g.CreatedDate

                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

                return gender is null ? Results.NotFound() : Results.Ok(gender);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in GenderEndpoint: /api/gender/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the gender with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/gender
        group.MapPost("/", async (
            GenderDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GenderEndpoint");

            try
            {
                var gender = dto.ToEntity();

                context.Genders.Add(gender);
                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/gender/{gender.Id}",
                    gender
                );
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in GenderEndpoint: /api/gender POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the gender.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // PUT: api/gender/{id}
        group.MapPut("/{id:int}", async (
            int id,
            EditGenderDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GenderEndpoint");

            try
            {
                var existingGender = await context.Genders.FindAsync(id);

                if (existingGender is null)
                {
                    return Results.NotFound();
                }

                existingGender.ToEntity(dto);

                context.Genders.Update(existingGender);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in GenderEndpoint: /api/gender/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the gender with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // DELETE: api/gender/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (
            int id,
            int loginUserId,
            DateTime deletedDate,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("GenderEndpoint");

            try
            {
                var existingGender = await context.Genders.FindAsync(id);

                if (existingGender is null)
                {
                    return Results.NotFound();
                }

                existingGender.IsDeleted = true;
                existingGender.UpdatedBy = loginUserId;
                existingGender.UpdatedDate = deletedDate;

                context.Genders.Update(existingGender);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in GenderEndpoint: /api/gender/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the gender with ID {id}.",
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