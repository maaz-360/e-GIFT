using eGift.Admin.Common;
using eGift.Admin.Helpers;
using eGift.Admin.Models.ListViewModels;
using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eGift.Admin.Controllers;

public class CategoryController : Controller
{
    #region Fields

    private readonly WebClientHelper _webClient;
    private readonly ILogger<CategoryController> _logger;

    #endregion

    #region Constructors

    public CategoryController(
        WebClientHelper webClient,
        ILogger<CategoryController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region Category Default CRUD

    // GET: Category
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = new CategoryListViewModel();

            categories.CategoryList = await _webClient
                .GetAsync<List<CategoryResponseViewModel>>(
                    "/api/category")
                ?? new List<CategoryResponseViewModel>();

            return View(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CategoryController Index: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View();
        }
    }

    // GET: Category/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var category = await _webClient
                .GetAsync<CategoryResponseViewModel>(
                    $"/api/category/{id}");

            if (category == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Category not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CategoryController Details/{id}: {Message}",
                id,
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Category/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CategoryViewModel());
    }

    // POST: Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryViewModel model)
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
                .PostAsync<CategoryViewModel, CategoryViewModel>(
                    "/api/category",
                    model);

            if (response == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create category.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to create category.";

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Category created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CategoryController Create POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(model);
        }
    }

    // GET: Category/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var category = await _webClient
                .GetAsync<CategoryViewModel>(
                    $"/api/category/{id}");

            if (category == null)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Category not found.";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CategoryController Edit GET: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        CategoryViewModel model)
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
                .PutAsync<CategoryViewModel, CategoryViewModel>(
                    $"/api/category/{id}",
                    model);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to update category.");

                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to update category.";

                return View(model);
            }

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Category updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CategoryController Edit POST: {Message}",
                ex.Message);

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                ex.Message;

            return View(model);
        }
    }

    // POST: Category/Delete/5
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
                $"/api/category/{id}" +
                $"?loginUserId={loginUserId}" +
                $"&deletedDate={DateTime.Now.ToDateTimeString()}");

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Category deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Exception in CategoryController Delete POST: {Message}",
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