using System.ComponentModel.DataAnnotations;

namespace eGift.Store.Models.ViewModels;

public class MyCartViewModel
{
    #region Data Model Properties
    public int Id { get; set; }

    [Display(Name = "Customer Name")]
    [Required(ErrorMessage = "This field is required.")]
    public int CustomerId { get; set; }

    [Display(Name = "Product Name")]
    [Required(ErrorMessage = "This field is required.")]
    public int ProductId { get; set; }

    [Display(Name = "Quantity")]
    [Required(ErrorMessage = "This field is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; } = 1;
    #endregion
}