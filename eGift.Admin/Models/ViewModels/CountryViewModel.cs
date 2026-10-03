using System.ComponentModel.DataAnnotations;

namespace eGift.Admin.Models.ViewModels;

public class CountryViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Country Code")]
    [Required(ErrorMessage = "This field is required.")]
    public string CountryCode { get; set; } = string.Empty;

    [Display(Name = "Country Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string CountryName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    #endregion
}