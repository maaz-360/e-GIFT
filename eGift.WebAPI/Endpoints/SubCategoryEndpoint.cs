using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class SubCategoryEndpoint
{
    public static RouteGroupBuilder MapSubCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/subcategory").WithTags("SubCategory");

        #region Default CRUD Endpoints

        // GET: api/subcategory
        group.MapGet("/", async (AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SubCategoryEndpoint");

            try
            {
                var subCategories = await (
                    from subCategory in context.SubCategories
                    join category in context.Categories on subCategory.CategoryId equals category.Id
                    where !subCategory.IsDeleted
                    select new
                    {
                        Id = subCategory.Id,
                        CategoryId = subCategory.CategoryId,
                        CategoryName = category.CategoryName,
                        SubCategoryName = subCategory.SubCategoryName,
                        Description = subCategory.Description,
                        CreatedDate = subCategory.CreatedDate
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return subCategories is null
                    ? Results.NotFound()
                    : Results.Ok(subCategories);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in SubCategoryEndpoint: /api/subcategory GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving subcategories.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/subcategory/{id}
        group.MapGet("/{id:int}", async (int id, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SubCategoryEndpoint");

            try
            {
                var subCategory = await (
                    from sc in context.SubCategories
                    join category in context.Categories on sc.CategoryId equals category.Id
                    where sc.Id == id && !sc.IsDeleted
                    select new
                    {
                        Id = sc.Id,
                        CategoryId = sc.CategoryId,
                        CategoryName = category.CategoryName,
                        SubCategoryName = sc.SubCategoryName,
                        Description = sc.Description,
                        CreatedDate = sc.CreatedDate

                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

                return subCategory is null
                    ? Results.NotFound()
                    : Results.Ok(subCategory);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in SubCategoryEndpoint: /api/subcategory/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the subcategory with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/subcategory
        group.MapPost("/", async (SubCategoryDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SubCategoryEndpoint");

            try
            {
                var subCategory = dto.ToEntity();

                context.SubCategories.Add(subCategory);
                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/subcategory/{subCategory.Id}",
                    subCategory
                );
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in SubCategoryEndpoint: /api/subcategory POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the subcategory.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // PUT: api/subcategory/{id}
        group.MapPut("/{id:int}", async ( int id,EditSubCategoryDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SubCategoryEndpoint");

            try
            {
                var existingSubCategory = await context.SubCategories.FindAsync(id);

                if (existingSubCategory is null)
                {
                    return Results.NotFound();
                }

                existingSubCategory.ToEntity(dto);

                context.SubCategories.Update(existingSubCategory);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in SubCategoryEndpoint: /api/subcategory/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the subcategory with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // DELETE: api/subcategory/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async ( int id, int loginUserId, DateTime deletedDate, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("SubCategoryEndpoint");

            try
            {
                var existingSubCategory = await context.SubCategories.FindAsync(id);

                if (existingSubCategory is null)
                {
                    return Results.NotFound();
                }

                existingSubCategory.IsDeleted = true;
                existingSubCategory.UpdatedBy = loginUserId;
                existingSubCategory.UpdatedDate = deletedDate;

                context.SubCategories.Update(existingSubCategory);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in SubCategoryEndpoint: /api/subcategory/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the subcategory with ID {id}.",
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