using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Models.ViewModels;

public class AddressViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Street 1")]
    [Required(ErrorMessage = "This field is required.")]
    public string Street1 { get; set; } = string.Empty;

    [Display(Name = "Street 2")]
    public string? Street2 { get; set; }

    [Display(Name = "Country")]
    [Required(ErrorMessage = "This field is required.")]
    public int CountryId { get; set; }

    [Display(Name = "State")]
    [Required(ErrorMessage = "This field is required.")]
    public int StateId { get; set; }

    [Display(Name = "City")]
    [Required(ErrorMessage = "This field is required.")]
    public int CityId { get; set; }

    [Display(Name = "Pin Code")]
    [Required(ErrorMessage = "This field is required.")]
    public string? PinCode { get; set; }

    #endregion

    #region View Model Properties

    [Display(Name = "Country Name")]
    public string? CountryName { get; set; }

    [Display(Name = "State Name")]
    public string? StateName { get; set; }

    [Display(Name = "City Name")]
    public string? CityName { get; set; }

    [Display(Name = "Full Address")]
    public string? FullAddress { get; set; }
    #endregion

    #region Select List Properties
    public SelectList? Countries { get; set; }
    public SelectList? States { get; set; }
    public SelectList? Cities { get; set; }
    #endregion
}