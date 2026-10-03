using eGift.Store.Common;
using eGift.Store.Models.ResponseViewModel;
using eGift.Store.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Store.Controllers;

public class CustomerController : Controller
{
    #region Fields
    private readonly WebClientHelper _webClient;
    private readonly ILogger<CustomerController> _logger;

    #endregion

    #region Constructor
    public CustomerController(WebClientHelper webClient,ILogger<CustomerController> logger)
    {
        _webClient = webClient;
        _logger = logger;
    }

    #endregion

    #region My Profile
    // GET: Customer/MyProfile
    [HttpGet]
    public async Task<IActionResult> MyProfile()
    {
        try
        {
            var customerId =HttpContext.Session.GetInt32("UserId");

            if (!customerId.HasValue)
            {
                TempData["ToastrType"] =ToastrType.Error.ToString();
                TempData["ToastrMessage"] ="Please login to view your profile.";

                return RedirectToAction("Index", "Home");
            }


            var customer =await _webClient.GetAsync<CustomerResponseViewModel>(
                    $"/api/customer/{customerId.Value}");

            if (customer == null)
            {
                TempData["ToastrType"] =ToastrType.Error.ToString();
                TempData["ToastrMessage"] ="Unable to load your profile.";

                return RedirectToAction("Index", "Home");
            }
            return View(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Exception in CustomerController MyProfile.");

            TempData["ToastrType"] =ToastrType.Error.ToString();
            TempData["ToastrMessage"] ="Unable to load your profile.";

            return RedirectToAction("Index", "Home");
        }
    }

    #endregion

    #region Edit Profile

    // GET: Customer/EditProfile
    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        try
        {
            var customerId =HttpContext.Session.GetInt32("UserId");

            if (!customerId.HasValue)
            {
                TempData["ToastrType"] =ToastrType.Error.ToString();
                TempData["ToastrMessage"] ="Please login to edit your profile.";

                return RedirectToAction("Index", "Home");
            }

          // Get Customer
            var response =await _webClient.GetAsync<CustomerResponseViewModel>(
                    $"/api/customer/{customerId.Value}");

            if (response == null)
            {
                TempData["ToastrType"] =ToastrType.Error.ToString();
                TempData["ToastrMessage"] ="Unable to load your profile.";

                return RedirectToAction(nameof(MyProfile));
            }

            // Create Customer ViewModel
            var model = new CustomerViewModel
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                DateofBirth = response.DateofBirth,
                GenderId = response.GenderId,
                Mobile = response.Mobile,
                Email = response.Email,
                AddressId = response.AddressId,
                IsActive = response.IsActive,
                ProfileImagePath =response.ProfileImagePath,
                ProfileImageData =response.ProfileImageData,
                RoleId = response.RoleId,
                IsDefault = response.IsDefault,
                Age = response.Age,
                GenderName =response.GenderName,
                AddressName =response.FullAddress,
                RoleName =response.RoleName,
                UserName = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };

        // Load Existing Address
            if (response.AddressId.HasValue &&
                response.AddressId.Value > 0)
            {
                var address =await _webClient.GetAsync<AddressViewModel>(
                        $"/api/address/{response.AddressId.Value}");


                if (address != null)
                {
                    model.Street1 =address.Street1;
                    model.Street2 =address.Street2;
                    model.CountryId =address.CountryId;
                    model.StateId =address.StateId;
                    model.CityId =address.CityId;
                    model.PinCode =address.PinCode;
                }
            }

            // Load Dropdowns
            await LoadGenders(model);
            await LoadAddressDropdowns(model);

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Exception in CustomerController EditProfile GET.");

            TempData["ToastrType"] =ToastrType.Error.ToString();
            TempData["ToastrMessage"] ="Unable to load profile for editing.";

            return RedirectToAction(nameof(MyProfile));
        }
    }

    // POST: Customer/EditProfile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(CustomerViewModel model)
    {
        try
        {
            // Get Logged-in Customer
            var customerId =HttpContext.Session.GetInt32("UserId");

            if (!customerId.HasValue)
            {
                TempData["ToastrType"] =ToastrType.Error.ToString();
                TempData["ToastrMessage"] ="Please login to edit your profile.";

                return RedirectToAction("Index", "Home");
            }

             if (customerId.Value != model.Id)
            {
                return BadRequest();
            }


         // Remove Unused Validation
            ModelState.Remove(nameof(model.Password));
            ModelState.Remove(nameof(model.ConfirmPassword));
            ModelState.Remove(nameof(model.UserName));

            
            bool addressStarted =!string.IsNullOrWhiteSpace(model.Street1)
                ||
                !string.IsNullOrWhiteSpace(
                    model.Street2)
                ||
                model.CountryId.HasValue
                ||
                model.StateId.HasValue
                ||
                model.CityId.HasValue
                ||
                !string.IsNullOrWhiteSpace(
                    model.PinCode);

            if (addressStarted)
            {
                if (string.IsNullOrWhiteSpace(model.Street1))
                {
                    ModelState.AddModelError(nameof(model.Street1),"Street 1 is required.");
                }
                if (!model.CountryId.HasValue ||model.CountryId.Value <= 0)
                {
                    ModelState.AddModelError(nameof(model.CountryId),"Country is required.");
                }

                if (!model.StateId.HasValue ||model.StateId.Value <= 0)
                {
                    ModelState.AddModelError(nameof(model.StateId),"State is required.");
                }
                if (!model.CityId.HasValue ||model.CityId.Value <= 0)
                {
                    ModelState.AddModelError(nameof(model.CityId),"City is required.");
                }
                if (string.IsNullOrWhiteSpace(model.PinCode))
                {
                    ModelState.AddModelError(nameof(model.PinCode),"Pin Code is required.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadGenders(model);
                await LoadAddressDropdowns(model);

                return View(model);
            }

            int? addressId =model.AddressId;

            bool hasAddress =!string.IsNullOrWhiteSpace(model.Street1) && model.CountryId.HasValue
                && model.StateId.HasValue && model.CityId.HasValue && !string.IsNullOrWhiteSpace(
                model.PinCode);


            if (hasAddress)
            {
            
                if (addressId.HasValue &&
                    addressId.Value > 0)
                {
                    var address =
                        new AddressViewModel
                        {
                            Id =addressId.Value,
                            Street1 =model.Street1!,
                            Street2 =model.Street2,
                            CountryId =model.CountryId ?? 0,
                            StateId =model.StateId ?? 0,
                            CityId =model.CityId ?? 0,
                            PinCode =model.PinCode,
                            IsDeleted =false,
                            UpdatedBy =customerId.Value,
                            UpdatedDate =DateTime.Now
                        };


                    var updateResult =await _webClient.PutAsync<AddressViewModel, AddressViewModel>(
        $"/api/address/{addressId.Value}",
        address);

                    if (!updateResult)
                    {
                        TempData["ToastrType"] =ToastrType.Error.ToString();
                        TempData["ToastrMessage"] ="Unable to update your address.";

                        await LoadGenders(model);
                        await LoadAddressDropdowns(model);

                        return View(model);
                    }
                }
                else
                {
                  
                    var address =
                        new AddressViewModel
                        {
                            Street1 =model.Street1!,
                            Street2 =model.Street2,
                            CountryId =model.CountryId ?? 0,
                            StateId =model.StateId ?? 0,

                            CityId =model.CityId ?? 0,
                            PinCode =model.PinCode,

                            IsDeleted =false,
                            CreatedBy =customerId.Value,

                            CreatedDate =DateTime.Now
                        };


                    var createdAddress =
     await _webClient.PostAsync<AddressViewModel, AddressViewModel>(
         "/api/address",
         address);

                    if (createdAddress == null ||
                        createdAddress.Id <= 0)
                    {
                        TempData["ToastrType"] =
                            ToastrType.Error.ToString();

                        TempData["ToastrMessage"] =
                            "Unable to create your address.";

                        await LoadGenders(model);

                        await LoadAddressDropdowns(model);

                        return View(model);
                    }


                    // ==================================
                    // IMPORTANT
                    // ==================================

                    addressId =
                        createdAddress.Id;

                    model.AddressId =
                        addressId;
                }
            }


            // ==========================================
            // CUSTOMER FORM DATA
            // ==========================================

            using var formData =
                new MultipartFormDataContent();


            formData.Add(
                new StringContent(
                    model.Id.ToString()),
                nameof(model.Id));


            formData.Add(
                new StringContent(
                    model.FirstName),
                nameof(model.FirstName));


            formData.Add(
                new StringContent(
                    model.LastName),
                nameof(model.LastName));


            if (model.DateofBirth.HasValue)
            {
                formData.Add(
                    new StringContent(
                        model.DateofBirth
                            .Value
                            .ToString("o")),
                    nameof(model.DateofBirth));
            }


            formData.Add(
                new StringContent(
                    model.GenderId.ToString()),
                nameof(model.GenderId));


            formData.Add(
                new StringContent(
                    model.Mobile),
                nameof(model.Mobile));


            if (!string.IsNullOrWhiteSpace(
                model.Email))
            {
                formData.Add(
                    new StringContent(
                        model.Email),
                    nameof(model.Email));
            }


            // ==========================================
            // ADDRESS ID
            // ==========================================

            if (addressId.HasValue &&
                addressId.Value > 0)
            {
                formData.Add(
                    new StringContent(
                        addressId.Value.ToString()),
                    nameof(model.AddressId));
            }


            formData.Add(
                new StringContent(
                    model.IsActive.ToString()),
                nameof(model.IsActive));


            formData.Add(
                new StringContent(
                    model.RoleId.ToString()),
                nameof(model.RoleId));


            formData.Add(
                new StringContent(
                    model.IsDefault.ToString()),
                nameof(model.IsDefault));


            formData.Add(
                new StringContent("false"),
                nameof(model.IsDeleted));


            formData.Add(
                new StringContent(
                    customerId.Value.ToString()),
                nameof(model.UpdatedBy));


            formData.Add(
                new StringContent(
                    DateTime.Now.ToString("o")),
                nameof(model.UpdatedDate));


            // ==========================================
            // PROFILE IMAGE
            // ==========================================

            if (model.ProfileImage != null &&
                model.ProfileImage.Length > 0)
            {
                var imageContent =
                    new StreamContent(
                        model.ProfileImage.OpenReadStream());


                imageContent.Headers.ContentType =
                    new System.Net.Http.Headers
                        .MediaTypeHeaderValue(
                            model.ProfileImage.ContentType);


                formData.Add(
                    imageContent,
                    nameof(model.ProfileImage),
                    model.ProfileImage.FileName);
            }


            // ==========================================
            // UPDATE CUSTOMER
            // ==========================================

            var success =
                await _webClient.PutFormAsync(
                    $"/api/customer/{customerId.Value}",
                    formData);


            if (!success)
            {
                TempData["ToastrType"] =
                    ToastrType.Error.ToString();

                TempData["ToastrMessage"] =
                    "Unable to update your profile.";

                await LoadGenders(model);

                await LoadAddressDropdowns(model);

                return View(model);
            }


            // ==========================================
            // SUCCESS
            // ==========================================

            TempData["ToastrType"] =
                ToastrType.Success.ToString();

            TempData["ToastrMessage"] =
                "Profile updated successfully.";


            return RedirectToAction(
                nameof(MyProfile));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in CustomerController EditProfile POST.");

            TempData["ToastrType"] =
                ToastrType.Error.ToString();

            TempData["ToastrMessage"] =
                "Unable to update your profile.";

            await LoadGenders(model);

            await LoadAddressDropdowns(model);

            return View(model);
        }
    }

    #endregion

    #region Image Retrieval

    // GET: Customer/Image/{fileName}
    [HttpGet]
    public async Task<IActionResult> Image(
        string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return NotFound();
            }


            var image =
                await _webClient.GetFileAsync(
                    $"/api/customer/image/{Uri.EscapeDataString(fileName)}");


            if (image == null ||
                image.Length == 0)
            {
                return NotFound();
            }


            var extension =
                Path.GetExtension(fileName)
                    .ToLowerInvariant();


            var contentType =
                extension switch
                {
                    ".jpg" or ".jpeg" =>
                        "image/jpeg",

                    ".png" =>
                        "image/png",

                    ".gif" =>
                        "image/gif",

                    ".webp" =>
                        "image/webp",

                    _ =>
                        "application/octet-stream"
                };


            return File(
                image,
                contentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in CustomerController Image.");

            return NotFound();
        }
    }

    #endregion

    #region AJAX Cascading Dropdowns

    // GET: Customer/GetStates?countryId=1
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
                ex,
                "Exception in CustomerController GetStates/{countryId}.",
                countryId);

            return StatusCode(500);
        }
    }

    // GET: Customer/GetCities?stateId=2
    [HttpGet]
    public async Task<IActionResult> GetCities(int stateId)
    {
        try
        {
            var cities = await _webClient
                .GetAsync<List<CityViewModel>>("/api/city");

            var filteredCities = (cities ?? [])
                .Where(x => x.StateId == stateId)
                .ToList();

            return Json(filteredCities);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in CustomerController GetCities/{stateId}.",
                stateId);

            return StatusCode(500);
        }
    }

    #endregion

    #region Private Methods

    private async Task LoadGenders(CustomerViewModel model)
    {
        try
        {
            var genders = await _webClient
                .GetAsync<List<GenderViewModel>>("/api/gender");

            model.Genders = new SelectList(
                genders ?? new List<GenderViewModel>(),
                "Id",
                "GenderName",
                model.GenderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in CustomerController LoadGenders.");

            model.Genders = new SelectList(
                Enumerable.Empty<GenderViewModel>());
        }
    }

    private async Task LoadAddressDropdowns(CustomerViewModel model)
    {
        var countries = await _webClient
            .GetAsync<List<CountryViewModel>>("/api/country");

        model.Countries = new SelectList(
            countries ?? new List<CountryViewModel>(),
            "Id",
            "CountryName",
            model.CountryId);


        if (model.CountryId.HasValue &&
            model.CountryId.Value > 0)
        {
            var states = await _webClient
                .GetAsync<List<StateViewModel>>("/api/state");

            var filteredStates = (states ?? [])
                .Where(x => x.CountryId == model.CountryId.Value)
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


        if (model.StateId.HasValue &&
            model.StateId.Value > 0)
        {
            var cities = await _webClient
                .GetAsync<List<CityViewModel>>("/api/city");

            var filteredCities = (cities ?? [])
                .Where(x => x.StateId == model.StateId.Value)
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