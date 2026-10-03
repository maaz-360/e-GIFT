using System.ComponentModel.DataAnnotations;

namespace eGift.Admin.Models.ViewModels;

public class OrderDetailsViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Order")]
    [Required(ErrorMessage = "This field is required.")]
    public int OrderId { get; set; }

    [Display(Name = "Product")]
    [Required(ErrorMessage = "This field is required.")]
    public int ProductId { get; set; }

    [Display(Name = "Unit Price")]
    [Required(ErrorMessage = "This field is required.")]
    public decimal UnitPrice { get; set; }

    [Display(Name = "Quantity")]
    [Required(ErrorMessage = "This field is required.")]
    public int Quantity { get; set; }

    [Display(Name = "Discount")]
    public decimal? Discount { get; set; }

    [Display(Name = "Tax")]
    public decimal? Tax { get; set; }

    [Display(Name = "Net Amount")]
    public decimal NetAmount { get; set; }

    #endregion
}