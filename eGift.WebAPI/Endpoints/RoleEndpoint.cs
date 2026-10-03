using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class RoleEndpoint
{
    public static RouteGroupBuilder MapRoleEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/role").WithTags("Role");

        #region Default CRUD Endpoints

        // GET: api/role
        group.MapGet("/", async (
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RoleEndpoint");

            try
            {
                var roles = await (
                    from role in context.Roles
                    where !role.IsDeleted
                    select new
                    {
                        Id = role.Id,
                        RoleName = role.RoleName,
                        Description = role.Description,
                        CreatedDate = role.CreatedDate
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return roles is null
                    ? Results.NotFound()
                    : Results.Ok(roles);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in RoleEndpoint: /api/role GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving roles.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/role/{id}
        group.MapGet("/{id:int}", async (
            int id,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RoleEndpoint");

            try
            {
                var role = await (
                    from r in context.Roles
                    where r.Id == id && !r.IsDeleted
                    select new
                    {
                        Id = r.Id,
                        RoleName = r.RoleName,
                        Description = r.Description,
                        CreatedDate = r.CreatedDate

                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

                return role is null
                    ? Results.NotFound()
                    : Results.Ok(role);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in RoleEndpoint: /api/role/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the role with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/role
        group.MapPost("/", async (
            RoleDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RoleEndpoint");

            try
            {
                var role = dto.ToEntity();

                context.Roles.Add(role);
                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/role/{role.Id}",
                    role
                );
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in RoleEndpoint: /api/role POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the role.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // PUT: api/role/{id}
        group.MapPut("/{id:int}", async (
            int id,
            EditRoleDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RoleEndpoint");

            try
            {
                var existingRole = await context.Roles.FindAsync(id);

                if (existingRole is null)
                {
                    return Results.NotFound();
                }

                existingRole.ToEntity(dto);

                context.Roles.Update(existingRole);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in RoleEndpoint: /api/role/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the role with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // DELETE: api/role/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (
            int id,
            int loginUserId,
            DateTime deletedDate,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("RoleEndpoint");

            try
            {
                var existingRole = await context.Roles.FindAsync(id);

                if (existingRole is null)
                {
                    return Results.NotFound();
                }

                existingRole.IsDeleted = true;
                existingRole.UpdatedBy = loginUserId;
                existingRole.UpdatedDate = deletedDate;

                context.Roles.Update(existingRole);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in RoleEndpoint: /api/role/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the role with ID {id}.",
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