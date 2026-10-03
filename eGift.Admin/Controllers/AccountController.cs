using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eGift.Admin.Controllers;

public class AccountController : Controller
{
    #region Fields
    private readonly WebClientHelper _webClient;
    private readonly ILogger<AccountController> _logger;
    #endregion

    #region Constructors
    public AccountController(WebClientHelper webClient, ILogger<AccountController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }
    #endregion

    #region Default Account Actions

    // GET : Index
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    #endregion

    #region Login Actions
    // POST : Login
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(SignInViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Index",model);
            }
            var response = await _webClient.GetAsync<LoginResponseViewModel>($"/api/login/employee?userName={model.UserName}&password={model.Password}");

            if (response == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to login.");

                TempData["ToastrType"] = ToastrType.Error.ToString();
                TempData["ToastrMessage"] = "Unable to login.";

                return View("Index", model);
            }

            if (response.Message != "Login successfully.")
            {
                ModelState.AddModelError(
                    string.Empty,
                    response.Message);

                TempData["ToastrType"] = ToastrType.Error.ToString();
                TempData["ToastrMessage"] = response.Message;

                return View("Index", model);
            }

            // Session values
            HttpContext.Session.SetInt32("UserId", response.UserId);

            HttpContext.Session.SetString("UserName", response.UserName);

            HttpContext.Session.SetInt32("RoleId", response.RoleId);

            TempData["ToastrType"] = ToastrType.Success.ToString();
            TempData["ToastrMessage"] = response.Message;

            // Login successful
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            // TODO: Log exception 
            _logger.LogError("Exception in AccountController Login: {Message}", ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;
        }
        return View("Index", model);
    }
    #endregion

    #region Logout Actions
    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        TempData["ToastrType"] = ToastrType.Success.ToString();
        TempData["ToastrMessage"] = "Logout successfully.";

        return RedirectToAction("Index", "Account");
    }
    #endregion
}
