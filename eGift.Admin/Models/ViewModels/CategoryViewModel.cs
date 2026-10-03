using System.ComponentModel.DataAnnotations;

namespace eGift.Admin.Models.ViewModels;

public class CategoryViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Category Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string CategoryName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    #endregion
}