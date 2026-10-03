using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Controllers;

public class CityController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<CityController> _logger;

    #endregion

    #region Constructors

    public CityController(
        WebClientHelper webClient,
        ILogger<CityController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region City Default CRUD

    // GET: City
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var cities = new CityListViewModel();

            cities.CityList = await _webClient
                .GetAsync<List<CityResponseViewModel>>(
                    "/api/city")
                ?? new List<CityResponseViewModel>();

            return View(cities);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CityController Index: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View();
        }
    }

    // GET: City/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var city = await _webClient
                .GetAsync<CityResponseViewModel>(
                    $"/api/city/{id}");

            if (city == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "City not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(city);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CityController Details/{id}: {Message}",
                id,
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // GET: City/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var model = new CityViewModel();

            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CityController Create GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: City/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CityViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(model);

                return View(model);
            }

            model.CreatedBy =
                Convert.ToInt32(
                    HttpContext.Session.GetInt32("UserId"));

            model.CreatedDate = DateTime.Now;

            var response = await _webClient
                .PostAsync<CityViewModel, CityViewModel>(
                    "/api/city",
                    model);

            if (response == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create city.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to create city.";

                await LoadDropdowns(model);

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "City created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CityController Create POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            await LoadDropdowns(model);

            return View(model);
        }
    }

    // GET: City/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var model = await _webClient
                .GetAsync<CityViewModel>(
                    $"/api/city/{id}");

            if (model == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "City not found.";

                return RedirectToAction(nameof(Index));
            }

            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CityController Edit GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: City/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        CityViewModel model)
    {
        try
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdowns(model);

                return View(model);
            }

            model.UpdatedBy =
                Convert.ToInt32(
                    HttpContext.Session.GetInt32("UserId"));

            model.UpdatedDate = DateTime.Now;

            var success = await _webClient
                .PutAsync<CityViewModel, CityViewModel>(
                    $"/api/city/{id}",
                    model);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to update city.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to update city.";

                await LoadDropdowns(model);

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "City updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CityController Edit POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            await LoadDropdowns(model);

            return View(model);
        }
    }

    // POST: City/Delete/5
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
                $"/api/city/{id}" +
                $"?loginUserId={loginUserId}" +
                $"&deletedDate={DateTime.Now.ToDateTimeString()}");

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "City deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CityController Delete POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region AJAX Cascading Dropdowns
    // GET: City/GetStates
    [HttpGet]
    public async Task<IActionResult> GetStates(int countryId)
    {
        try
        {
            var states = await _webClient
                .GetAsync<List<StateViewModel>>("/api/state");

            var filteredStates = (states ?? [])
                .Where(x => x.CountryId == countryId)
                .ToList();

            return Json(filteredStates);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CityController GetStates/{countryId}: {Message}",
                countryId,
                ex.Message);

            return StatusCode(500);
        }
    }
    #endregion

    #region Private Methods

    private async Task LoadDropdowns(CityViewModel model)
    {
        var countries = await _webClient
            .GetAsync<List<CountryViewModel>>("/api/country");

        model.Countries = new SelectList(
            countries ?? [],
            "Id",
            "CountryName",
            model.CountryId);

        // Load states when editing
        if (model.CountryId > 0)
        {
            var states = await _webClient
                .GetAsync<List<StateViewModel>>("/api/state");

            var filteredStates = (states ?? [])
                .Where(x => x.CountryId == model.CountryId)
                .ToList();

            model.States = new SelectList(
                filteredStates,
                "Id",
                "StateName",
                model.StateId);
        }
        else
        {
            model.States = new SelectList(
           Enumerable.Empty<StateViewModel>());
        }
    }

    #endregion
}