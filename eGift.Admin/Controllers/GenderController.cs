using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eGift.Admin.Controllers;

public class GenderController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<GenderController> _logger;

    #endregion

    #region Constructors

    public GenderController(
        WebClientHelper webClient,
        ILogger<GenderController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region Gender Default CRUD

    // GET: Gender
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var genders = await _webClient
                .GetAsync<List<GenderResponseViewModel>>(
                    "/api/gender");

            var model = new GenderListViewModel
            {
                GenderList = genders ?? new List<GenderResponseViewModel>()
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in GenderController Index: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(new GenderListViewModel());
        }
    }

  

    // GET: Gender/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var gender = await _webClient
                .GetAsync<GenderResponseViewModel>(
                    $"/api/gender/{id}");

            if (gender is null)
            {
                return NotFound();
            }

            return View(gender);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in GenderController Details: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

  

    // GET: Gender/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new GenderViewModel());
    }

    // POST: Gender/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GenderViewModel model)
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

            await _webClient.PostAsync<GenderViewModel, GenderResponseViewModel>(
                "/api/gender",
                model);

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Gender created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in GenderController Create POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(model);
        }
    }

    

    // GET: Gender/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var gender = await _webClient
                .GetAsync<GenderViewModel>(
                    $"/api/gender/{id}");

            if (gender is null)
            {
                return NotFound();
            }

            return View(gender);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in GenderController Edit GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Gender/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        GenderViewModel model)
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
                GenderViewModel,
                GenderViewModel>(
                $"/api/gender/{id}",
                model);

            if (!response)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to update gender.";

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Gender updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in GenderController Edit POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(model);
        }
    }

   

    // POST: Gender/Delete/5
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
                $"/api/gender/{id}" +
                $"?loginUserId={loginUserId}" +
                $"&deletedDate={Uri.EscapeDataString(deletedDate.ToString("O"))}");

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Gender deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in GenderController Delete: {Message}",
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