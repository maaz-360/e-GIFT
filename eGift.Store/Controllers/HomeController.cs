using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using eGift.Store.Common;
using eGift.Store.Models.ViewModels;
using eGift.Store.Models.ResponseViewModel;

namespace eGift.Store.Controllers;

public class HomeController : Controller
{
    #region Fields
    private readonly WebClientHelper _webClient;
    private readonly ILogger<HomeController> _logger;
    #endregion

    #region Constructors
    public HomeController(WebClientHelper webClient, ILogger<HomeController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }
    #endregion

    #region Home Actions
    // GET: Home
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        try
        {
            var list = await _webClient.GetAsync<List<ProductResponseViewModel>>("/api/product") ?? new List<ProductResponseViewModel>();

            return View(list);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in EmployeeController Index: {Message}", ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            return View();
        }
    }
    #endregion

    #region Image Retrieval
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Image(string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return NotFound();
            }

            var image = await _webClient.GetFileAsync(
                $"/api/product/image/{Uri.EscapeDataString(fileName)}");

            if (image == null || image.Length == 0)
            {
                return NotFound();
            }

            var extension = Path.GetExtension(fileName)
                .ToLowerInvariant();

            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

            return File(image, contentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in EmployeeController Image.");

            return NotFound();
        }
    }
    #endregion

    #region Product Details

    // GET: Home/ProductDetails/5
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ProductDetails(int id)
    {
        try
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var product = await _webClient.GetAsync<ProductResponseViewModel>($"/api/product/{id}");

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Exception in HomeController ProductDetails.");

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = "Unable to load product details.";

            return RedirectToAction("Index");
        }
    }

    #endregion

    #region Privacy Action
    public IActionResult Privacy()
    {
        return View();
    }
    #endregion

    #region Error Action
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    #endregion
}
