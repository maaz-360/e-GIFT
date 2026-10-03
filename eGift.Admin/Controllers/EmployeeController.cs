
using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Controllers;

public class EmployeeController : Controller
{
    #region Fields
    private readonly WebClientHelper _webClient;
    private readonly ILogger<EmployeeController> _logger;
    #endregion

    #region Constructors
    public EmployeeController(WebClientHelper webClient,ILogger<EmployeeController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }
    #endregion

    #region Employee Default CRUD
    // GET: Employee
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var employees = new EmployeeListViewModel();
            employees.EmployeeList = await _webClient
                .GetAsync<List<EmployeeResponseViewModel>>("/api/employee") ?? new List<EmployeeResponseViewModel>();

            return View(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in EmployeeController Index: {Message}", ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            return View();
        }
    }

    // GET: Employee/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var employee = await _webClient
                .GetAsync<EmployeeResponseViewModel>(
                    $"/api/employee/{id}");

            if (employee == null)
            {
                TempData["ToastrType"] = ToastrType.Error.ToString();
                TempData["ToastrMessage"] = "Employee not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in EmployeeController Details/{id}: {Message}", id, ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Employee/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var model = new EmployeeViewModel();
            
            // Load dropdown data here
            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in EmployeeController Create GET: {Message}", ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Employee/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                // Load dropdown data here
                await LoadDropdowns(model);
                return View(model);
            }

            model.CreatedBy= Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            model.CreatedDate= DateTime.Now;

            using var formData = new MultipartFormDataContent();
            formData.Add(
                        new StringContent(model.Id.ToString()),
                        nameof(model.Id));

            formData.Add(
                new StringContent(model.FirstName),
                nameof(model.FirstName));

            formData.Add(
                new StringContent(model.LastName),
                nameof(model.LastName));

            formData.Add(
                new StringContent(model.DateofBirth.ToDateTimeString()),
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

            if (model.AddressId.HasValue)
            {
                formData.Add(
                    new StringContent(model.AddressId.Value.ToString()),
                    nameof(model.AddressId));
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
                new StringContent(model.IsDeleted.ToString()),
                nameof(model.IsDeleted));

            formData.Add(
                new StringContent(model.CreatedBy.ToString()),
                nameof(model.CreatedBy));

            formData.Add(
                new StringContent(model.CreatedDate.ToDateTimeString()),
                nameof(model.CreatedDate));

            // Image
            if (model.ProfileImage != null)
            {
                var imageContent =
                    new StreamContent(model.ProfileImage.OpenReadStream());

                imageContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(
                        model.ProfileImage.ContentType);

                formData.Add(
                    imageContent,
                    nameof(model.ProfileImage),
                    model.ProfileImage.FileName);
            }

            var response = await _webClient.PostFormAsync<EmployeeResponseViewModel>(
                    "/api/employee", formData);

            if (response == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create employee.");

                TempData["ToastrType"] = ToastrType.Error.ToString();
                TempData["ToastrMessage"] = "Unable to create employee.";

                // Load dropdown data here
                await LoadDropdowns(model);
                return View(model);
            }

            // Create login for the employee
            var loginModel = new LoginViewModel
            {
                RefId = response.Id,
                RefType = RefType.Employee.ToString(),
                UserName = model.UserName,
                Password = model.Password,
                RoleId = model.RoleId,
                IsActive = true,
                LogInDate = null,
                LastLoginDate = null,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now
            };

            var loginResponse = await _webClient
                .PostAsync<LoginViewModel, LoginViewModel>(
                    "/api/login",
                    loginModel);

            if (loginResponse == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create employee login.");

                TempData["ToastrType"] = ToastrType.Error.ToString();
                TempData["ToastrMessage"] = "Unable to create employee login.";

                // Load dropdown data here
                await LoadDropdowns(model);
                return View(model);
            }

            TempData["ToastrType"] = ToastrType.Success.ToString();
            TempData["ToastrMessage"] = "Employee created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in EmployeeController Create POST: {Message}", ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            // Load dropdown data here
            await LoadDropdowns(model);
            return View(model);
        }
    }

    // GET: Employee/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var model = await _webClient
                .GetAsync<EmployeeViewModel>(
                    $"/api/employee/{id}");

            if (model == null)
            {
                TempData["ToastrType"] = ToastrType.Error.ToString();
                TempData["ToastrMessage"] = "Employee not found.";

                return RedirectToAction(nameof(Index));
            }

            // Load dropdown data here
            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in EmployeeController Edit GET: {Message}", ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Employee/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
    {
        try
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                // Load dropdown data here
                await LoadDropdowns(model);

                return View(model);
            }

            model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            model.UpdatedDate = DateTime.Now;

            using var formData = new MultipartFormDataContent();

            formData.Add(
                new StringContent(model.Id.ToString()),
                nameof(model.Id));

            formData.Add(
                new StringContent(model.FirstName),
                nameof(model.FirstName));

            formData.Add(
                new StringContent(model.LastName),
                nameof(model.LastName));

            formData.Add(
                new StringContent(
                    model.DateofBirth.ToDateTimeString()),
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

            if (model.AddressId.HasValue)
            {
                formData.Add(
                    new StringContent(model.AddressId.Value.ToString()),
                    nameof(model.AddressId));
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
                new StringContent(model.IsDeleted.ToString()),
                nameof(model.IsDeleted));

            formData.Add(
                new StringContent(model.ProfileImagePath ?? string.Empty),
                nameof(model.ProfileImagePath));

            formData.Add(
                new StringContent(model.UpdatedBy.Value.ToString()),
                nameof(model.UpdatedBy));

            formData.Add(
                new StringContent(model.UpdatedDate.ToDateTimeString()),
                nameof(model.UpdatedDate));

            // Profile Image
            if (model.ProfileImage != null &&
                model.ProfileImage.Length > 0)
            {
                var imageContent =
                    new StreamContent(
                        model.ProfileImage.OpenReadStream());

                imageContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(
                        model.ProfileImage.ContentType);

                formData.Add(
                    imageContent,
                    nameof(model.ProfileImage),
                    model.ProfileImage.FileName);
            }

            var response =
            await _webClient.PutFormAsync<object>(
                $"/api/employee/{id}",
                formData);

            // Update login for the employee
            var loginModel = new LoginViewModel
            {
                Id = model.LoginId,
                RefId = model.Id,
                RefType = RefType.Employee.ToString(),
                UserName = model.UserName,
                Password = model.Password,
                RoleId = model.RoleId,
                IsActive = true,
                LogInDate = null,
                LastLoginDate = null,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = DateTime.Now
            };           

            var loginResponse = await _webClient
                .PutAsync<LoginViewModel, LoginViewModel>(
                    $"/api/login/{loginModel.Id}",
                    loginModel);

            TempData["ToastrType"] = ToastrType.Success.ToString();
            TempData["ToastrMessage"] = "Employee updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in EmployeeController Edit POST: {Message}", ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            // Load dropdown data here
            await LoadDropdowns(model);

            return View(model);
        }
    }

    // POST: Employee/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            int loginUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            await _webClient.DeleteAsync(
                $"/api/employee/{id}?loginUserId={loginUserId}&deletedDate={DateTime.Now.ToDateTimeString()}");

            TempData["ToastrType"] = ToastrType.Success.ToString();
            TempData["ToastrMessage"] = "Employee deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception in EmployeeController Delete POST: {Message}", ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
    #endregion

    #region Image Retrieval
    [HttpGet]
    public async Task<IActionResult> Image(string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return NotFound();
            }

            var image = await _webClient.GetFileAsync(
                $"/api/employee/image/{Uri.EscapeDataString(fileName)}");

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

    #region Private Methods
    private async Task LoadDropdowns(EmployeeViewModel model)
    {
        var addresses = await _webClient
        .GetAsync<List<AddressViewModel>>("/api/address");

        var genders = await _webClient
            .GetAsync<List<GenderViewModel>>("/api/gender");

        var roles = await _webClient
            .GetAsync<List<RoleViewModel>>("/api/role");

        model.Addresses = new SelectList(
            addresses ?? [],
            "Id",
            "FullAddress",
            model.AddressId);

        model.Genders = new SelectList(
            genders ?? [],
            "Id",
            "GenderName",
            model.GenderId);

        model.Roles = new SelectList(
            roles ?? [],
            "Id",
            "RoleName",
            model.RoleId);

    }
    #endregion
}