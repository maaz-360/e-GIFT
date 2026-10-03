using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class CategoryEndpoint
{
    public static RouteGroupBuilder MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/category").WithTags("Category");

        #region Default CRUD Endpoints
        // GET: api/categories
        group.MapGet("/", async (AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CategoryEndpoint");
            try
            {
                var categories = await (
                    from category in context.Categories
                    where !category.IsDeleted
                    select new
                    {
                        Id = category.Id,
                        CategoryName = category.CategoryName,
                        Description = category.Description,
                        CreatedDate = category.CreatedDate
                    }
                    ).AsNoTracking()
                    .ToListAsync();

                return categories is null ? Results.NotFound() : Results.Ok(categories);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError("Exception in CategoryEndpoint: /api/categories GET: {Message}.", ex.Message);
                return Results.Json(new
                {
                    Message = "An error occurred while retrieving categories.",
                    error = ex.Message
                },
                statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        // GET: api/categories/{id}
        group.MapGet("/{id:int}", async (int id, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CategoryEndpoint");
            try
            {
                var category = await (
                    from c in context.Categories
                    where c.Id == id && !c.IsDeleted
                    select new
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        Description = c.Description,
                        CreatedDate = c.CreatedDate,

                    }
                    ).AsNoTracking()
                    .FirstOrDefaultAsync();
                return category is null ? Results.NotFound() : Results.Ok(category);
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

        // POST: api/categories
        group.MapPost("/", async (CategoryDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CategoryEndpoint");
            try
            {
                var category = dto.ToEntity();
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                return Results.Created($"/api/categories/{category.Id}", category);
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError("Exception in CategoryEndpoint: /api/categories POST: {Message}.", ex.Message);
                return Results.Json(new
                {
                    Message = "An error occurred while creating the category.",
                    error = ex.Message
                },
                statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        // PUT: api/category/{id}
        group.MapPut("/{id:int}", async (int id, EditCategoryDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CategoryEndpoint");
            try
            {
                var existingCategory = await context.Categories.FindAsync(id);
                if (existingCategory == null)
                {
                    return Results.NotFound();
                }

                existingCategory.ToEntity(dto);
                context.Categories.Update(existingCategory);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError("Exception in CategoryEndpoint: /api/categories/{id} PUT: {Message}.", id, ex.Message);
                return Results.Json(new
                {
                    Message = $"An error occurred while updating the category with ID {id}.",
                    error = ex.Message
                },
                statusCode: StatusCodes.Status500InternalServerError);
            }
        });

        // // DELETE: api/category/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (
            int id,
            int loginUserId,
            DateTime deletedDate,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("CategoryEndpoint");

            try
            {
                var existingCategory = await context.Categories.FindAsync(id);

                if (existingCategory is null)
                {
                    return Results.NotFound();
                }

                existingCategory.IsDeleted = true;
                existingCategory.UpdatedBy = loginUserId;
                existingCategory.UpdatedDate = deletedDate;

                context.Categories.Update(existingCategory);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception and return a generic error response
                logger.LogError(
                    "Exception in CategoryEndpoint: /api/category/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the category with ID {id}.",
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