using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Models.ViewModels;

public class OrderViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Customer")]
    [Required(ErrorMessage = "This field is required.")]
    public int CustomerId { get; set; }

    [Display(Name = "Total Amount")]
    [Required(ErrorMessage = "This field is required.")]
    public decimal TotalAmount { get; set; }

    [Display(Name = "Total Discount")]
    public decimal? TotalDiscount { get; set; }

    [Display(Name = "Total Tax")]
    public decimal? TotalTax { get; set; }

    [Display(Name = "Order Number")]
    public string OrderNumber { get; set; } = string.Empty;

    [Display(Name = "Notes")]
    public string? Notes { get; set; }

    [Display(Name = "Dispatched Date")]
    public DateTime? DispatchedDate { get; set; }

    [Display(Name = "Shipped Date")]
    public DateTime? ShippedDate { get; set; }

    [Display(Name = "Delivery Date")]
    public DateTime? DeliveryDate { get; set; }

    [Display(Name = "Cancel Date")]
    public DateTime? CancelDate { get; set; }

    [Display(Name = "Status")]
    [Required(ErrorMessage = "This field is required.")]
    public int StatusId { get; set; }

    #endregion

    #region View Model Properties

    [Display(Name = "Customer Name")]
    public string? CustomerName { get; set; }

    [Display(Name = "Status Name")]
    public string? StatusName { get; set; }

    #endregion

    #region Select List Properties
    public SelectList? Customers { get; set; }
    public SelectList? Statuses { get; set; }
    #endregion
}