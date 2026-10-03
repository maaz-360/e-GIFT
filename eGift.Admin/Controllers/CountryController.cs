using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eGift.Admin.Controllers;

public class CountryController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<CountryController> _logger;

    #endregion

    #region Constructors

    public CountryController(WebClientHelper webClient, ILogger<CountryController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region Country Default CRUD

    // GET: Country
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var countries = new CountryListViewModel();

            countries.CountryList = await _webClient.GetAsync<List<CountryResponseViewModel>>("/api/country") ?? new List<CountryResponseViewModel>();

            return View(countries);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CountryController Index: {Message}",
                ex.Message);

            TempData["ToastrType"] = ToastrType.Error.ToString();
            TempData["ToastrMessage"] = ex.Message;

            return View();
        }
    }

    // GET: Country/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var country = await _webClient
                .GetAsync<CountryResponseViewModel>(
                    $"/api/country/{id}");

            if (country == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Country not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(country);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CountryController Details/{id}: {Message}",
                id,
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Country/Create
    [HttpGet]
    public IActionResult Create()
    {
        try
        {
            var model = new CountryViewModel();

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CountryController Create GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Country/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CountryViewModel model)
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

            var response = await _webClient
                .PostAsync<CountryViewModel, CountryViewModel>(
                    "/api/country",
                    model);

            if (response == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create country.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to create country.";

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Country created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CountryController Create POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(model);
        }
    }

    // GET: Country/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var model = await _webClient
                .GetAsync<CountryViewModel>(
                    $"/api/country/{id}");

            if (model == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Country not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CountryController Edit GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Country/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        CountryViewModel model)
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

            var success = await _webClient
                .PutAsync<CountryViewModel, CountryViewModel>(
                    $"/api/country/{id}",
                    model);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to update country.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to update country.";

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Country updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CountryController Edit POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(model);
        }
    }

    // POST: Country/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            int loginUserId =
                Convert.ToInt32(
                    HttpContext.Session.GetInt32("UserId"));

            await _webClient.DeleteAsync(
                $"/api/country/{id}" +
                $"?loginUserId={loginUserId}" +
                $"&deletedDate={DateTime.Now.ToDateTimeString()}");

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Country deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CountryController Delete POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    #endregion
}