using eGift.Store.Common;
using eGift.Store.Helpers;
using eGift.Store.Models.ListViewModels;
using eGift.Store.Models.ResponseViewModel;
using eGift.Store.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eGift.Store.Controllers;

public class OrderController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<OrderController> _logger;

    #endregion

    #region Constructor

    public OrderController(WebClientHelper webClient, ILogger<OrderController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region Place Order

    // POST: Order/PlaceOrder
    [HttpPost]
    public async Task<IActionResult> PlaceOrder(List<OrderDetailsViewModel> Items)
    {
        try
        {
            // Get logged-in customer ID from Session
            var customerId = HttpContext.Session.GetInt32("UserId");

            if (!customerId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    message = "Please login to place order."
                });
            }

            // Create Order ViewModel
            var model = new OrderViewModel
            {
                CustomerId = customerId.Value,
                CreatedBy = customerId.Value,
                CreatedDate = DateTime.Now,
                Items = Items
            };

            Items.ForEach(x =>
            {
                x.CreatedBy = customerId.Value;
                x.CreatedDate = DateTime.Now;
            });

            // Calling API Mycart/api/mycart
            var response = await _webClient.PostAsync<OrderViewModel, OrderViewModel>("/api/order",
                    model);

            if (response == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Unable to place order."
                });
            }

            return Json(new
            {
                success = true,
                message = "Order placed successfully.",
                data = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in OderController PlaceOrder.");

            return Json(new
            {
                success = false,
                message = "An error occurred while placing order."
            });
        }
    }

    #endregion

    #region My Order

    // GET: Order/MyOrder
    [HttpGet]
    public async Task<IActionResult> MyOrder()
    {
        try
        {
            // Get logged-in customer ID
            var customerId = HttpContext.Session.GetInt32("UserId");

            if (!customerId.HasValue)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Please login to view your orders.";

                return RedirectToAction("Index", "Home");
            }

            List<OrderResponseViewModel>? orders;

            try
            {
                // Get all orders from API
                orders =
                    await _webClient.GetAsync<
                        List<OrderResponseViewModel>>(
                        "/api/order");
                        
            }
            catch (HttpRequestException)
            {
                // Treat API 404 as empty order list
                orders = new List<OrderResponseViewModel>();
            }

            // Filter orders for logged-in customer
            orders = orders?
                .Where(x => x.CustomerId == customerId.Value)
                .OrderByDescending(x => x.CreatedDate)
                .ToList()
                ?? new List<OrderResponseViewModel>();

            // Create List ViewModel
            var model = new OrderListViewModel
            {
                OrderList = orders.Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerName,

                    TotalAmount = x.TotalAmount,
                    TotalDiscount = x.TotalDiscount,
                    TotalTax = x.TotalTax,

                    OrderNumber = x.OrderNumber ?? string.Empty,
                    Notes = x.Notes,

                    CreatedDate = x.CreatedDate,

                    DispatchedDate = x.DispatchedDate,
                    ShippedDate = x.ShippedDate,
                    DeliveryDate = x.DeliveryDate,
                    CancelDate = x.CancelDate,

                    StatusId = x.StatusId,
                    StatusName = GetStatusName(x.StatusId)
                }).ToList()
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in OrderController MyOrder.");

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                "Unable to load your orders.";

            return RedirectToAction("Index", "Home");
        }
    }

    #endregion

    #region Cancel Order

    // POST: Order/CancelOrder
    [HttpPost]
    public async Task<IActionResult> CancelOrder(int id)
    {
        try
        {
            // Get logged-in customer ID
            var customerId = HttpContext.Session.GetInt32("UserId");

            if (!customerId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    message = "Please login to cancel your order."
                });
            }

            if (id <= 0)
            {
                return Json(new
                {
                    success = false,
                    message = "Invalid order."
                });
            }

            // Get all orders
            var orders = await _webClient.GetAsync<List<OrderResponseViewModel>>("/api/order");

            // Find the customer's order
            var order = orders?
                .FirstOrDefault(x =>
                    x.Id == id &&
                    x.CustomerId == customerId.Value);

            if (order == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Order not found."
                });
            }

            // Check order status
            if (order.StatusId != (int)OrderStatus.New)
            {
                return Json(new
                {
                    success = false,
                    message = "Only new orders can be cancelled."
                });
            }

            // Call Order Status API
            var apiUrl =
                $"/api/order/{id}/status" +
                $"?statusId={(int)OrderStatus.Cancelled}" +
                $"&loginUserId={customerId.Value}" +
                $"&updatedDate={DateTime.Now.ToDateTimeString()}";

            var response = await _webClient.PutAsync<object, object>(
                apiUrl,
                new { });

            if (!response)
            {
                return Json(new
                {
                    success = false,
                    message = "Unable to cancel the order."
                });
            }

            return Json(new
            {
                success = true,
                message = "Order cancelled successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in OrderController CancelOrder.");

            return Json(new
            {
                success = false,
                message = "An error occurred while cancelling the order."
            });
        }
    }

    #endregion

    #region Order Status

    private static string GetStatusName(int statusId)
    {
        return Enum.IsDefined(typeof(OrderStatus), statusId)
            ? ((OrderStatus)statusId).ToString()
            : "Unknown";
    }

    #endregion
}