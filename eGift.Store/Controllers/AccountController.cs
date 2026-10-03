using eGift.Store.Common;
using eGift.Store.Models.ResponseViewModel;
using eGift.Store.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Store.Controllers;

public class AccountController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<AccountController> _logger;

    #endregion

    #region Constructors

    public AccountController(
        WebClientHelper webClient,
        ILogger<AccountController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region Default Account Actions

    // GET : Index
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    #endregion

    #region Login Actions
    // GET : Login
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View(new SignInViewModel());
    }

    // POST : Login
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(SignInViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _webClient.GetAsync<LoginResponseViewModel>(
                    $"/api/login/customer?userName={Uri.EscapeDataString(model.UserName)}&password={Uri.EscapeDataString(model.Password)}");

            if (response == null)
            {
                ModelState.AddModelError(string.Empty,"Unable to login.");

                TempData["ToastrType"] =ToastrType.Error.ToString();
                TempData["ToastrMessage"] ="Unable to login.";

                return View(model);
            }

            if (response.Message != "Login successfully.")
            {
                ModelState.AddModelError(string.Empty,response.Message);

                TempData["ToastrType"] =ToastrType.Error.ToString();
                TempData["ToastrMessage"] =response.Message;

                return View(model);
            }

            // Session
            HttpContext.Session.SetInt32("UserId",response.UserId);
            HttpContext.Session.SetString("UserName",response.UserName);
            HttpContext.Session.SetInt32("RoleId",response.RoleId);

            TempData["ToastrType"] =ToastrType.Success.ToString();
            TempData["ToastrMessage"] =response.Message;

            return RedirectToAction("Index","Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Exception in AccountController Login.");

            TempData["ToastrType"] =ToastrType.Error.ToString();
            TempData["ToastrMessage"] =ex.Message;

            return View(model);
        }
    }
    // POST : PopupLogin
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PopupLogin(SignInViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Please enter username and password."
                });
            }

            var response =
                await _webClient.GetAsync<LoginResponseViewModel>(
                    $"/api/login/customer?userName={Uri.EscapeDataString(model.UserName)}&password={Uri.EscapeDataString(model.Password)}");

            if (response == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Unable to login."
                });
            }

            if (response.Message != "Login successfully.")
            {
                return Json(new
                {
                    success = false,
                    message = response.Message
                });
            }

            // Session
            HttpContext.Session.SetInt32("UserId",response.UserId);
            HttpContext.Session.SetString("UserName",response.UserName);
            HttpContext.Session.SetInt32("RoleId",response.RoleId);

            return Json(new
            {
                success = true,
                message = response.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in AccountController PopupLogin.");

            return Json(new
            {
                success = false,
                message = "Username or password invalid."
            });
        }
    }
    #endregion

    #region Register Actions

    // GET : Register
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        try
        {
            var model = new CustomerViewModel();

            var genders = await _webClient
                .GetAsync<List<GenderViewModel>>(
                    "/api/gender");

            model.Genders = new SelectList(genders ?? [],"Id","GenderName",model.GenderId);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in AccountController Register GET: {Message}",ex.Message);

            TempData["ToastrType"] =ToastrType.Error.ToString();
            TempData["ToastrMessage"] =ex.Message;

            return RedirectToAction("Index","Home");
        }
    }

    // POST : Register
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(CustomerViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadGenderDropdown(model);

                return View(model);
            }

            // Set Customer defaults
            model.IsActive = true;
            model.IsDefault = false;
            model.RoleId = 3;

            // Create Customer
            using var formData = new MultipartFormDataContent();

            formData.Add(
                new StringContent(model.FirstName),
                nameof(model.FirstName));

            formData.Add(
                new StringContent(model.LastName),
                nameof(model.LastName));

            formData.Add(
                new StringContent(
                    model.DateofBirth?.ToString("yyyy-MM-dd") ?? string.Empty),
                nameof(model.DateofBirth));

            formData.Add(
                new StringContent(model.GenderId.ToString()),
                nameof(model.GenderId));

            formData.Add(
                new StringContent(model.Mobile),
                nameof(model.Mobile));

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                formData.Add(
                    new StringContent(model.Email),
                    nameof(model.Email));
            }

            formData.Add(
                new StringContent(model.IsActive.ToString()),
                nameof(model.IsActive));

            formData.Add(
                new StringContent(model.RoleId.ToString()),
                nameof(model.RoleId));

            formData.Add(
                new StringContent(model.IsDefault.ToString()),
                nameof(model.IsDefault));

            formData.Add(
                new StringContent("false"),
                nameof(model.IsDeleted));

            formData.Add(
                new StringContent("0"),
                nameof(model.CreatedBy));

            formData.Add(
                new StringContent(DateTime.Now.ToString("o")),
                nameof(model.CreatedDate));

            var customerResponse =
                await _webClient.PostFormAsync<CustomerResponseViewModel>(
                    "/api/customer",
                    formData);

            if (customerResponse == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create customer.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to create customer.";

                await LoadGenderDropdown(model);

                return View(model);
            }

            // Create Login
            var loginModel = new LoginViewModel
            {
                RefId = customerResponse.Id,
                RefType = "Customer",
                UserName = model.UserName,
                Password = model.Password,
                RoleId = model.RoleId,
                IsActive = true,
                LogInDate = null,
                LastLoginDate = null
            };

            var loginResponse =
                await _webClient.PostAsync<LoginViewModel, LoginResponseViewModel>(
                    "/api/login",
                    loginModel);

            if (loginResponse == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Customer was created, but login could not be created.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Customer was created, but login could not be created.";

                await LoadGenderDropdown(model);

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Registration successful. Please login.";

            return RedirectToAction(
                nameof(Login));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in AccountController Register POST.");

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            await LoadGenderDropdown(model);

            return View(model);
        }
    }

    #endregion

    #region Logout Actions

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        TempData["ToastrType"] =ToastrType.Success.ToString();
        TempData["ToastrMessage"] ="Logout successfully.";

        return RedirectToAction("Index","Home");
    }

    #endregion

    #region Private Methods

    private async Task LoadGenderDropdown(CustomerViewModel model)
    {
        var genders = await _webClient
            .GetAsync<List<GenderViewModel>>(
                "/api/gender");

        model.Genders = new SelectList(
            genders ?? [],
            "Id",
            "GenderName",
            model.GenderId);
    }

    #endregion
}

