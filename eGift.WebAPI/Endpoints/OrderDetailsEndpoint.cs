using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class OrderDetailsEndpoint
{
    public static RouteGroupBuilder MapOrderDetailsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/orderdetails").WithTags("OrderDetails");

        #region Default CRUD Endpoints

        // GET: api/orderdetails
        group.MapGet("/", async ( AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("OrderDetailsEndpoint");

            try
            {
                var orderDetails = await (
                    from orderDetail in context.OrderDetails
                    where !orderDetail.IsDeleted
                    select new
                    {
                        Id = orderDetail.Id,
                        OrderId = orderDetail.OrderId,

                        ProductId = orderDetail.ProductId,

                        UnitPrice = orderDetail.UnitPrice,
                        Quantity = orderDetail.Quantity,
                        Discount = orderDetail.Discount,
                        Tax = orderDetail.Tax,
                        NetAmount = orderDetail.NetAmount,

                        CreatedDate = orderDetail.CreatedDate
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return orderDetails is null
                    ? Results.NotFound()
                    : Results.Ok(orderDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in OrderDetailsEndpoint: /api/orderdetails GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving order details.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/orderdetails/{id}
        group.MapGet("/{id:int}", async (int id, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("OrderDetailsEndpoint");

            try
            {
                var orderDetail = await (
                    from od in context.OrderDetails
                    where od.Id == id && !od.IsDeleted
                    select new
                    {
                        Id = od.Id,

                        OrderId = od.OrderId,

                        ProductId = od.ProductId,

                        UnitPrice = od.UnitPrice,
                        Quantity = od.Quantity,
                        Discount = od.Discount,
                        Tax = od.Tax,
                        NetAmount = od.NetAmount
                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

                return orderDetail is null
                    ? Results.NotFound()
                    : Results.Ok(orderDetail);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in OrderDetailsEndpoint: /api/orderdetails/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the order detail with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/orderdetails
        group.MapPost("/", async (
            OrderDetailsDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("OrderDetailsEndpoint");

            try
            {
                var orderDetail = dto.ToEntity();

                context.OrderDetails.Add(orderDetail);
                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/orderdetails/{orderDetail.Id}",
                    orderDetail
                );
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in OrderDetailsEndpoint: /api/orderdetails POST: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the order detail.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // PUT: api/orderdetails/{id}
        group.MapPut("/{id:int}", async (int id,EditOrderDetailsDto dto, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("OrderDetailsEndpoint");

            try
            {
                var existingOrderDetail = await context.OrderDetails.FindAsync(id);

                if (existingOrderDetail is null)
                {
                    return Results.NotFound();
                }

                existingOrderDetail.ToEntity(dto);

                context.OrderDetails.Update(existingOrderDetail);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in OrderDetailsEndpoint: /api/orderdetails/{id} PUT: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the order detail with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // DELETE: api/orderdetails/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (  int id,int loginUserId, DateTime deletedDate, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("OrderDetailsEndpoint");

            try
            {
                var existingOrderDetail = await context.OrderDetails.FindAsync(id);

                if (existingOrderDetail is null)
                {
                    return Results.NotFound();
                }

                existingOrderDetail.IsDeleted = true;
                existingOrderDetail.UpdatedBy = loginUserId;
                existingOrderDetail.UpdatedDate = deletedDate;

                context.OrderDetails.Update(existingOrderDetail);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in OrderDetailsEndpoint: /api/orderdetails/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the order detail with ID {id}.",
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