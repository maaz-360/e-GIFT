using System.ComponentModel.DataAnnotations;

namespace eGift.Admin.Models.ViewModels;

public class GenderViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Gender Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string GenderName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    #endregion
}