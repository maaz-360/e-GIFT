using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Models.ViewModels;

public class StateViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "State Code")]
    [Required(ErrorMessage = "This field is required.")]
    public string StateCode { get; set; } = string.Empty;

    [Display(Name = "State Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string StateName { get; set; } = string.Empty;

    [Display(Name = "Country")]
    [Required(ErrorMessage = "This field is required.")]
    public int CountryId { get; set; }

    [Display(Name = "Description")]
    public string? Description { get; set; }

    #endregion

    #region View Model Properties

    [Display(Name = "Country Name")]
    public string? CountryName { get; set; }

    #endregion

    #region Select List Properties

    public SelectList? Countries { get; set; }

    #endregion
}