using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Controllers;

public class StateController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<StateController> _logger;

    #endregion

    #region Constructors

    public StateController(
        WebClientHelper webClient,
        ILogger<StateController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region State Default CRUD

    // GET: State
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var states = new StateListViewModel();

            states.StateList = await _webClient
                .GetAsync<List<StateResponseViewModel>>(
                    "/api/state")
                ?? new List<StateResponseViewModel>();

            return View(states);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in StateController Index: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View();
        }
    }

    // GET: State/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var state = await _webClient
                .GetAsync<StateResponseViewModel>(
                    $"/api/state/{id}");

            if (state == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "State not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(state);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in StateController Details/{id}: {Message}",
                id,
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // GET: State/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var model = new StateViewModel();

            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in StateController Create GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: State/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StateViewModel model)
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
                .PostAsync<StateViewModel, StateViewModel>(
                    "/api/state",
                    model);

            if (response == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create state.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to create state.";

                await LoadDropdowns(model);

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "State created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in StateController Create POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            await LoadDropdowns(model);

            return View(model);
        }
    }

    // GET: State/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var model = await _webClient
                .GetAsync<StateViewModel>(
                    $"/api/state/{id}");

            if (model == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "State not found.";

                return RedirectToAction(nameof(Index));
            }

            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in StateController Edit GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: State/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        StateViewModel model)
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
                .PutAsync<StateViewModel, StateViewModel>(
                    $"/api/state/{id}",
                    model);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to update state.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to update state.";

                await LoadDropdowns(model);

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "State updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in StateController Edit POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            await LoadDropdowns(model);

            return View(model);
        }
    }

    // POST: State/Delete/5
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
                $"/api/state/{id}" +
                $"?loginUserId={loginUserId}" +
                $"&deletedDate={DateTime.Now.ToDateTimeString()}");

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "State deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in StateController Delete POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    #endregion

    #region Private Methods

    private async Task LoadDropdowns(StateViewModel model)
    {
        var countries = await _webClient
            .GetAsync<List<CountryViewModel>>(
                "/api/country");

        model.Countries = new SelectList(
            countries ?? [],
            "Id",
            "CountryName",
            model.CountryId);
    }

    #endregion
}