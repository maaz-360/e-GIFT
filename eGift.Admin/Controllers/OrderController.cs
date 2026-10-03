using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Controllers;

public class OrderController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<OrderController> _logger;

    #endregion

    #region Constructors

    public OrderController(
        WebClientHelper webClient,
        ILogger<OrderController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region Order Default CRUD
    // GET: Order
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var orders = new OrderListViewModel();

            orders.OrderList = await _webClient.GetAsync<List<OrderResponseViewModel>>(
                    "/api/order")?? new List<OrderResponseViewModel>();

            return View(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in OrderController Index: {Message}", ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            return View();
        }
    }

    // GET: Order/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var order = await _webClient.GetAsync<OrderResponseViewModel>($"/api/order/{id}");
            await LoadDropdowns(order);

            if (order == null)
            {
                TempData["ToastrType"] = ToastrType.Error.ToString();
                TempData["ToastrMessage"] = "Order not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in AddressController Details/{id}: {Message}",
                id, ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }
    #endregion

    #region Ajax Methods
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int orderId, int statusId)
    {
        try
        {
            var loginUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var updatedDate = DateTime.Now;

            var url = $"/api/order/{orderId}/status" +
                      $"?statusId={statusId}" +
                      $"&loginUserId={loginUserId}" +
                      $"&updatedDate={Uri.EscapeDataString(updatedDate.ToDateTimeString())}";

            var result = await _webClient.PutAsync<object, object>(
                        url,
                         new { }
                    );

            if (!result)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to update order status."
                });
            }

            return Json(new
            {
                success = true,
                message = "Order status updated successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in OrderController UpdateStatus/{orderId}: {Message}",
                orderId,ex.Message);

            return Json(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
    #endregion

    #region Private Methods
    private async Task LoadDropdowns(OrderResponseViewModel? order)
    {
        order!.Statuses = new SelectList(Enum.GetValues<OrderStatus>()
            .Select(x => new
            {
                Id = (int)x,
                Name = x.ToString()
            }), "Id", "Name", (int?)order.StatusId);
    }
    #endregion
}