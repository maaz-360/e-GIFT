using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Controllers;

public class SubCategoryController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<SubCategoryController> _logger;

    #endregion

    #region Constructors

    public SubCategoryController(
        WebClientHelper webClient,
        ILogger<SubCategoryController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region SubCategory Default CRUD

    // GET: SubCategory
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var subCategories = new SubCategoryListViewModel();

            subCategories.SubCategoryList = await _webClient
                .GetAsync<List<SubCategoryResponseViewModel>>(
                    "/api/subcategory")
                ?? new List<SubCategoryResponseViewModel>();

            return View(subCategories);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in SubCategoryController Index: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View();
        }
    }

    // GET: SubCategory/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var subCategory = await _webClient
                .GetAsync<SubCategoryResponseViewModel>(
                    $"/api/subcategory/{id}");

            if (subCategory == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Sub-category not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(subCategory);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in SubCategoryController Details/{id}: {Message}",
                id,
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // GET: SubCategory/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var model = new SubCategoryViewModel();

            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in SubCategoryController Create GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: SubCategory/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubCategoryViewModel model)
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
                .PostAsync<SubCategoryViewModel, SubCategoryViewModel>(
                    "/api/subcategory",
                    model);

            if (response == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create sub-category.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to create sub-category.";

                await LoadDropdowns(model);

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Sub-category created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in SubCategoryController Create POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            await LoadDropdowns(model);

            return View(model);
        }
    }

    // GET: SubCategory/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var model = await _webClient
                .GetAsync<SubCategoryViewModel>(
                    $"/api/subcategory/{id}");

            if (model == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Sub-category not found.";

                return RedirectToAction(nameof(Index));
            }

            await LoadDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in SubCategoryController Edit GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: SubCategory/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        SubCategoryViewModel model)
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
                .PutAsync<SubCategoryViewModel, SubCategoryViewModel>(
                    $"/api/subcategory/{id}",
                    model);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to update sub-category.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to update sub-category.";

                await LoadDropdowns(model);

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Sub-category updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in SubCategoryController Edit POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            await LoadDropdowns(model);

            return View(model);
        }
    }

    // POST: SubCategory/Delete/5
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
                $"/api/subcategory/{id}" +
                $"?loginUserId={loginUserId}" +
                $"&deletedDate={DateTime.Now.ToDateTimeString()}");

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Sub-category deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in SubCategoryController Delete POST: {Message}",
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

    private async Task LoadDropdowns(SubCategoryViewModel model)
    {
        var categories = await _webClient
            .GetAsync<List<CategoryViewModel>>(
                "/api/category");

        model.Categories = new SelectList(
            categories ?? [],
            "Id",
            "CategoryName",
            model.CategoryId);
    }

    #endregion
}