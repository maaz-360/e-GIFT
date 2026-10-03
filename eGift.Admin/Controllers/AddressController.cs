using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Controllers;

public class AddressController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<AddressController> _logger;

    #endregion

    #region Constructors

    public AddressController(
        WebClientHelper webClient,
        ILogger<AddressController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region Address Default CRUD

    // GET: Address
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var addresses = new AddressListViewModel();

            addresses.AddressList = await _webClient
                .GetAsync<List<AddressResponseViewModel>>(
                    "/api/address")
                ?? new List<AddressResponseViewModel>();

            return View(addresses);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in AddressController Index: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View();
        }
    }

    // GET: Address/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var address = await _webClient
                .GetAsync<AddressResponseViewModel>(
                    $"/api/address/{id}");

            if (address == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Address not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(address);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in AddressController Details/{id}: {Message}",
                id,
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Address/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var model = new AddressViewModel();

            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in AddressController Create GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Address/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddressViewModel model)
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
                .PostAsync<AddressViewModel, AddressViewModel>(
                    "/api/address",
                    model);

            if (response == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create address.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to create address.";

                await LoadDropdowns(model);

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Address created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in AddressController Create POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            await LoadDropdowns(model);

            return View(model);
        }
    }

    // GET: Address/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var model = await _webClient
                .GetAsync<AddressViewModel>(
                    $"/api/address/{id}");

            if (model == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Address not found.";

                return RedirectToAction(nameof(Index));
            }

            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in AddressController Edit GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Address/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        AddressViewModel model)
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
                .PutAsync<AddressViewModel, AddressViewModel>(
                    $"/api/address/{id}",
                    model);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to update address.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to update address.";

                await LoadDropdowns(model);

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Address updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in AddressController Edit POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            await LoadDropdowns(model);

            return View(model);
        }
    }

    // POST: Address/Delete/5
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
                $"/api/address/{id}" +
                $"?loginUserId={loginUserId}" +
                $"&deletedDate={DateTime.Now.ToDateTimeString()}");

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Address deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in AddressController Delete POST: {Message}",
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
    

    // GET: Address/GetStates?countryId=1
    [HttpGet]
    public async Task<IActionResult> GetStates(int countryId)
    {
        try
        {
            var states = await _webClient
                .GetAsync<List<StateViewModel>>(
                    "/api/state");

            var filteredStates = (states ?? [])
                .Where(x => x.CountryId == countryId)
                .ToList();

            return Json(filteredStates);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in AddressController GetStates/{countryId}: {Message}",
                countryId,
                ex.Message);

            return StatusCode(500);
        }
    }

    // GET: Address/GetCities?stateId=2
    [HttpGet]
    public async Task<IActionResult> GetCities(int stateId)
    {
        try
        {
            var cities = await _webClient
                .GetAsync<List<CityViewModel>>(
                    "/api/city");

            var filteredCities = (cities ?? [])
                .Where(x => x.StateId == stateId)
                .ToList();

            return Json(filteredCities);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in AddressController GetCities/{stateId}: {Message}",
                stateId,
                ex.Message);

            return StatusCode(500);
        }
    }

    #endregion

    #region Private Methods

    private async Task LoadDropdowns(AddressViewModel model)
    {
        var countries = await _webClient
            .GetAsync<List<CountryViewModel>>(
                "/api/country");

        model.Countries = new SelectList(
            countries ?? new List<CountryViewModel>(),
            "Id",
            "CountryName",
            model.CountryId);

        // Load states for Edit / validation failure
        if (model.CountryId > 0)
        {
            var states = await _webClient
                .GetAsync<List<StateViewModel>>(
                    "/api/state");

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

        // Load cities for Edit / validation failure
        if (model.StateId > 0)
        {
            var cities = await _webClient
                .GetAsync<List<CityViewModel>>(
                    "/api/city");

            var filteredCities = (cities ?? [])
                .Where(x => x.StateId == model.StateId)
                .ToList();

            model.Cities = new SelectList(
                filteredCities,
                "Id",
                "CityName",
                model.CityId);
        }
        else
        {
            model.Cities = new SelectList(
                Enumerable.Empty<CityViewModel>());
        }
    }

    #endregion
}