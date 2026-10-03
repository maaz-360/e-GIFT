using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Models.ViewModels;

public class CityViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "City Code")]
    [Required(ErrorMessage = "This field is required.")]
    public string CityCode { get; set; } = string.Empty;

    [Display(Name = "City Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string CityName { get; set; } = string.Empty;

    [Display(Name = "State")]
    [Required(ErrorMessage = "This field is required.")]
    public int StateId { get; set; }

    [Display(Name = "Description")]
    public string? Description { get; set; }

    #endregion

    #region View Model Properties

    [Display(Name = "State Name")]
    public string? StateName { get; set; }

    [Display(Name = "Country Name")]
    public string? CountryName { get; set; }

    [Display(Name = "Country")]
    public int CountryId { get; set; }

    #endregion

    #region Select List Properties
    public SelectList? States { get; set; }

    public SelectList? Countries { get; set; }

    #endregion
}