using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Models.ViewModels;

public class ProductViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Category")]
    [Required(ErrorMessage = "This field is required.")]
    public int CategoryId { get; set; }

    [Display(Name = "Sub Category")]
    [Required(ErrorMessage = "This field is required.")]
    public int SubCategoryId { get; set; }

    [Display(Name = "Quantity Per Unit")]
    [Required(ErrorMessage = "This field is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity per unit must be at least 1.")]

    public int QuantityPerUnit { get; set; }

    [Display(Name = "Unit Price")]
    [Required(ErrorMessage = "This field is required.")]
    [Range(typeof(decimal), "0.01", "999999999.99",
    ErrorMessage = "Unit price must be greater than 0.")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Size")]
    // [Required(ErrorMessage = "This field is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a size.")]
    public int? SizeId { get; set; }

    [Display(Name = "Discount")]
    [Required(ErrorMessage = "This field is required.")]
    [Range(typeof(decimal), "0", "100",
    ErrorMessage = "Discount must be between 0 and 100.")]
    public decimal? Discount { get; set; }

    [Display(Name = "Unit In Stock")]
    [Range(0, int.MaxValue, ErrorMessage = "Units in stock cannot be negative.")]
    [Required(ErrorMessage = "This field is required.")]
    public int UnitInStock { get; set; }

    [Display(Name = "Unit In Order")]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Units in order cannot be negative.")]
    public int UnitInOrder { get; set; }

    [Display(Name = "Product Available")]
    [Required(ErrorMessage = "This field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Product available cannot be negative.")]
    public int ProductAvailable { get; set; }

    [Display(Name = "Short Description")]
    public string? ShortDescription { get; set; }

    [Display(Name = "Long Description")]
    public string? LongDescription { get; set; }

    #endregion


    #region Existing Image Properties

    // Existing Main Product Image
    [Display(Name = "Product Image")]
    public string? ProductImagePath { get; set; }

    // Existing Additional Product Images
    [Display(Name = "Picture 1")]
    public string? PicturePath1 { get; set; }

    [Display(Name = "Picture 2")]
    public string? PicturePath2 { get; set; }

    [Display(Name = "Picture 3")]
    public string? PicturePath3 { get; set; }

    [Display(Name = "Picture 4")]
    public string? PicturePath4 { get; set; }

    #endregion


    #region Uploaded Image Properties

    // New Main Product Image
    [Display(Name = "Product Image")]
    public IFormFile? ProductImage { get; set; }

    // New Additional Product Images
    [Display(Name = "Picture 1")]
    public IFormFile? Picture1 { get; set; }

    [Display(Name = "Picture 2")]
    public IFormFile? Picture2 { get; set; }

    [Display(Name = "Picture 3")]
    public IFormFile? Picture3 { get; set; }

    [Display(Name = "Picture 4")]
    public IFormFile? Picture4 { get; set; }

    #endregion


    #region View Model Properties

    [Display(Name = "Category Name")]
    public string? CategoryName { get; set; }

    [Display(Name = "SubCategory Name")]
    public string? SubCategoryName { get; set; }

    [Display(Name = "Size Name")]
    public string? SizeName { get; set; }

    #endregion


    #region Select List Properties

    public SelectList? Categories { get; set; }

    public SelectList? SubCategories { get; set; }

    public SelectList? Sizes { get; set; }

    #endregion
}