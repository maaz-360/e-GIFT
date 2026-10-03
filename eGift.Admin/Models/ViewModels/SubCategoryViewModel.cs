using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Models.ViewModels;

public class SubCategoryViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Category")]
    [Required(ErrorMessage = "This field is required.")]
    public int CategoryId { get; set; }

    [Display(Name = "Sub Category Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string SubCategoryName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    #endregion

    #region View Model Properties

    [Display(Name = "Category Name")]
    public string? CategoryName { get; set; }

    #endregion

    #region Select List Properties

    public SelectList? Categories { get; set; }

    #endregion
}