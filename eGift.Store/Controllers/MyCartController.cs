using eGift.Store.Common;
using eGift.Store.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using eGift.Store.Models.ListViewModels;
using eGift.Store.Models.ResponseViewModel;
using eGift.Store.Helpers;

namespace eGift.Store.Controllers;

public class MyCartController : Controller
{
    #region Fields
    
    private readonly WebClientHelper _webClient;
    private readonly ILogger<MyCartController> _logger;

    #endregion

    #region Constructor
    public MyCartController(WebClientHelper webClient,ILogger<MyCartController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }
    #endregion

    #region Add To Cart
    
    // POST: MyCart/AddToCart
    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity=1)
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
                    message = "Please login to add product to cart."
                });
            }

            // Create MyCart ViewModel
            var model = new MyCartViewModel
            {
                ProductId = productId,
                CustomerId = customerId.Value,
                Quantity = quantity,
            };

            // Calling API Mycart/api/mycart
            var response =await _webClient.PostAsync<MyCartViewModel, MyCartViewModel>("/api/mycart",
                    model);

            if (response == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Unable to add product to cart."
                });
            }

            return Json(new
            {
                success = true,
                message = "Product added to cart successfully.",
                data = response
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Exception in MyCartController AddToCart.");

            return Json(new
            {
                success = false,
                message = "An error occurred while adding the product to cart."
            });
        }
    }
    
    #endregion

    #region My Cart
    
    // GET: MyCart
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // Get logged-in customer ID
            var customerId = HttpContext.Session.GetInt32("UserId");

            if (!customerId.HasValue)
            {
                TempData["ToastrType"] = ToastrType.Error.ToString();
                TempData["ToastrMessage"] = "Please login to view your cart.";

                return RedirectToAction("Index", "Home");
            }

            List<MyCartResponseViewModel>? cartItems;

            try
            {
                // Get cart items
                cartItems = await _webClient.GetAsync<List<MyCartResponseViewModel>>(
                        $"/api/mycart/customer/{customerId.Value}");
            }
            catch (HttpRequestException)
            {
                // Treat 404 as an empty cart instead of an error.
                cartItems = new List<MyCartResponseViewModel>();
            }

            // Create List ViewModel
            var model = new MyCartListViewModel
            {
                MyCartList = cartItems ?? new List<MyCartResponseViewModel>()
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in MyCartController Index.");

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = "Unable to load your cart.";

            return RedirectToAction("Index", "Home");
        }
    }
    
    #endregion

    #region Update Quantity
    
    // POST: MyCart/UpdateQuantity
    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int id,int productId,int quantity)
    {
        try
        {
            // Get logged-in customer
            var customerId = HttpContext.Session.GetInt32("UserId");

            if (!customerId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    message = "Please login to update your cart."
                });
            }

            // Validate quantity
            if (quantity < 1)
            {
                return Json(new
                {
                    success = false,
                    message = "Quantity must be at least 1."
                });
            }

            // Create request 
            var request = new
            {
                Id = id,
                ProductId = productId,
                CustomerId = customerId.Value,
                Quantity = quantity,
                IsDeleted = false,
                UpdatedBy = customerId.Value,
                UpdatedDate = DateTime.Now
            };

            // Call WebAPI
            var success = await _webClient.PutAsync<object, object>($"/api/mycart/{id}", request);

            if (!success)
            {
                return Json(new
                {
                    success = false,
                    message = "Unable to update cart quantity."
                });
            }

            return Json(new
            {
                success = true,
                message = "Cart quantity updated successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in MyCartController UpdateQuantity.");

            return Json(new
            {
                success = false,
                message = "An error occurred while updating the cart."
            });
        }
    }
    
    #endregion

    #region Remove / Clear Cart
    
    // POST: MyCart/RemoveItem
    // clearAll = false → remove one item
    // clearAll = true  → remove entire cart
    [HttpPost]
    public async Task<IActionResult> RemoveItem(int id, bool clearAll = false)
    {
        try
        {
            var customerId =
                HttpContext.Session.GetInt32("UserId");

            if (!customerId.HasValue)
            {
                return Json(new
                {
                    success = false,
                    message = "Please login to manage your cart."
                });
            }

            // REMOVE ONE ITEM
            if (!clearAll)
            {
                if (id <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid cart item."
                    });
                }

                await _webClient.DeleteAsync(
                    $"/api/mycart/{id}" +
                    $"?loginUserId={customerId.Value}" +
                   $"&deletedDate={DateTime.Now.ToDateTimeString()}");

                return Json(new
                {
                    success = true,
                    message = "Product removed from cart successfully."
                });
            }

            // CLEAR ENTIRE CART
            List<MyCartResponseViewModel> cartItems;
            try
            {
                cartItems = await _webClient.GetAsync<
                        List<MyCartResponseViewModel>>($"/api/mycart/customer/{customerId.Value}")
                    ?? new List<MyCartResponseViewModel>();
            }
            catch (HttpRequestException)
            {
                cartItems = new List<MyCartResponseViewModel>();
            }   

            if (!cartItems.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "Your cart is already empty."
                });
            }

            // Delete every cart item
            foreach (var item in cartItems)
            {
                await _webClient.DeleteAsync(
                    $"/api/mycart/{item.Id}" +
                    $"?loginUserId={customerId.Value}" +
                    $"&deletedDate={DateTime.Now.ToDateTimeString()}");
            }

            return Json(new
            {
                success = true,
                message = "Cart cleared successfully."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Exception in MyCartController RemoveItem.");

            return Json(new
            {
                success = false,
                message = "An error occurred while managing your cart."
            });
        }
    }
    
    #endregion
}