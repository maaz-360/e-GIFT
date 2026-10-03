using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eGift.Admin.Controllers;

public class RoleController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<RoleController> _logger;

    #endregion

    #region Constructors

    public RoleController(
        WebClientHelper webClient,
        ILogger<RoleController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region Role Default CRUD

    // GET: Role
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var roles = await _webClient
                .GetAsync<List<RoleResponseViewModel>>(
                    "/api/role");

            var model = new RoleListViewModel
            {
                RoleList = roles ?? new List<RoleResponseViewModel>()
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in RoleController Index: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(new RoleListViewModel());
        }
    }

   

    // GET: Role/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var role = await _webClient
                .GetAsync<RoleResponseViewModel>(
                    $"/api/role/{id}");

            if (role is null)
            {
                return NotFound();
            }

            return View(role);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in RoleController Details: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    

    // GET: Role/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new RoleViewModel());
    }

    // POST: Role/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoleViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedBy =
                Convert.ToInt32(
                    HttpContext.Session.GetInt32("UserId"));

            model.CreatedDate = DateTime.Now;

            await _webClient.PostAsync<RoleViewModel, RoleResponseViewModel>(
                "/api/role",
                model);

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Role created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in RoleController Create POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(model);
        }
    }

   

    // GET: Role/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var role = await _webClient
                .GetAsync<RoleViewModel>(
                    $"/api/role/{id}");

            if (role is null)
            {
                return NotFound();
            }

            return View(role);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in RoleController Edit GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Role/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        RoleViewModel model)
    {
        try
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UpdatedBy =
                Convert.ToInt32(
                    HttpContext.Session.GetInt32("UserId"));

            model.UpdatedDate = DateTime.Now;

            var response = await _webClient.PutAsync<
                RoleViewModel,
                RoleViewModel>(
                $"/api/role/{id}",
                model);

            if (!response)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to update role.";

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Role updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in RoleController Edit POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(model);
        }
    }

   

    // POST: Role/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var loginUserId =
                Convert.ToInt32(
                    HttpContext.Session.GetInt32("UserId"));

            var deletedDate = DateTime.Now;

            await _webClient.DeleteAsync(
                $"/api/role/{id}" +
                $"?loginUserId={loginUserId}" +
                $"&deletedDate={Uri.EscapeDataString(deletedDate.ToString("O"))}");

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Role deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in RoleController Delete: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    #endregion
}