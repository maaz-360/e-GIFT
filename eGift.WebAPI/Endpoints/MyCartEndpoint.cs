using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class MyCartEndpoint
{
    public static RouteGroupBuilder MapMyCartEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/mycart").WithTags("MyCart");

        #region Default CRUD Endpoints

        // GET: api/mycart
        group.MapGet("/", async (AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("MyCartEndpoint");

            try
            {
                var carts = await (
                    from cart in context.MyCarts
                    where !cart.IsDeleted
                    select new
                    {
                        Id = cart.Id,
                        ProductId = cart.ProductId,
                        CustomerId = cart.CustomerId,
                        Quantity = cart.Quantity,
                        CreatedDate = cart.CreatedDate
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return carts is null ? Results.NotFound() : Results.Ok(carts);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in MyCartEndpoint: /api/mycart GET: {Message}.",
                    ex.Message
                );
                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving cart items.",
                        error = ex.Message
                    }, statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/mycart/{id}
        group.MapGet("/{id:int}", async (int id, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("MyCartEndpoint");

            try
            {
                var cart = await (
                    from c in context.MyCarts
                    where c.Id == id && !c.IsDeleted
                    select new
                    {
                        Id = c.Id,
                        ProductId = c.ProductId,
                        CustomerId = c.CustomerId,
                        Quantity = c.Quantity,
                        CreatedDate = c.CreatedDate
                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

                return cart is null ? Results.NotFound() : Results.Ok(cart);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in MyCartEndpoint: /api/mycart/{id} GET: {Message}.",
                    id, ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the cart item with ID {id}.",
                        error = ex.Message
                    }, statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/mycart
        group.MapPost("/", async (MyCartDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("MyCartEndpoint");
            try
            {

                // Check customer
                var customerExists = await context.Customers
                    .AnyAsync(x => x.Id == dto.CustomerId && !x.IsDeleted);

                if (!customerExists)
                {
                    return Results.BadRequest(new
                    {
                        Message = $"Customer with ID {dto.CustomerId} does not exist or has been deleted."
                    });
                }
                // Checking the same product already exists in cart
                var existingCart = await context.MyCarts.FirstOrDefaultAsync
                    (x => x.CustomerId == dto.CustomerId && x.ProductId == dto.ProductId && !x.IsDeleted && !x.IsOrdered);

                if (existingCart is not null)
                {
                    // Increase the quantity.
                    existingCart.Quantity += dto.Quantity;

                    context.MyCarts.Update(existingCart);
                    await context.SaveChangesAsync();

                    return Results.Ok(existingCart);
                }

                //If Product does not exist in cart then Create a new item.
                var cart = dto.ToEntity();
                context.MyCarts.Add(cart);
                await context.SaveChangesAsync();

                return Results.Created($"/api/mycart/{cart.Id}", cart
                );
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in MyCartEndpoint: /api/mycart POST: {Message}.", ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while adding the product to the cart.",
                        error = ex.Message
                    }, statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // PUT: api/mycart/{id}
        group.MapPut("/{id:int}", async (int id, EditMyCartDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("MyCartEndpoint");

            try
            {
                var existingCart = await context.MyCarts.FirstOrDefaultAsync
                (x => x.Id == id && !x.IsDeleted);

                if (existingCart is null)
                {
                    return Results.NotFound();
                }

                existingCart.ToEntity(dto);

                context.MyCarts.Update(existingCart);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in MyCartEndpoint: /api/mycart/{id} PUT: {Message}.", id, ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the cart item with ID {id}.",
                        error = ex.Message
                    }, statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // DELETE: api/mycart/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (int id, int loginUserId, DateTime deletedDate, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("MyCartEndpoint");

            try
            {
                var existingCart = await context.MyCarts.FindAsync(id);

                if (existingCart is null)
                {
                    return Results.NotFound();
                }

                existingCart.IsDeleted = true;
                existingCart.UpdatedBy = loginUserId;
                existingCart.UpdatedDate = deletedDate;

                context.MyCarts.Update(existingCart);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in MyCartEndpoint: /api/mycart/{id} DELETE: {Message}.",
                    id, ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the cart item with ID {id}.",
                        error = ex.Message
                    }, statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        #endregion

        #region Customer My Cart Endpoints
        // GET: api/mycart/customer/1
        group.MapGet("/customer/{cid:int}", async (int cid, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("MyCartEndpoint");

            try
            {
                var carts = await (
                    from cart in context.MyCarts
                    join p in context.Products on cart.ProductId equals p.Id
                    where cart.CustomerId == cid && !cart.IsDeleted && !cart.IsOrdered
                    select new
                    {
                        Id = cart.Id,
                        ProductId = cart.ProductId,
                        CustomerId = cart.CustomerId,
                        Quantity = cart.Quantity,
                        CreatedDate = cart.CreatedDate,

                        ProductName = p.Name,
                        ProductImagePath = p.ProductImagePath,
                        ProductPrice = p.UnitPrice,
                        ShortDescription = p.ShortDescription
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return carts.Count == 0 ? Results.NotFound() : Results.Ok(carts);
            }
            catch (Exception ex)
            {
                logger.LogError("Exception in MyCartEndpoint: /api/mycart GET: {Message}.",
                    ex.Message
                );
                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving cart items.",
                        error = ex.Message
                    }, statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });
        #endregion

        return group;
    }
}