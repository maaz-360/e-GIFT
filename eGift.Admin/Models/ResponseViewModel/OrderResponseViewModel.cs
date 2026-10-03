using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Models.ResponseViewModel;

public class OrderResponseViewModel
{
    #region Response View Model Properties

    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string? Notes { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? DispatchedDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? CancelDate { get; set; }

    public int StatusId { get; set; }
    public string? StatusName { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal? TotalTax { get; set; }

    #endregion

    #region List View Model Properties
    public List<OrderDetailsResponseViewModel> OrderDetailsList { get; set; } = new List<OrderDetailsResponseViewModel>();
    #endregion

    #region Select Lists
    public SelectList? Statuses { get; set; }

    #endregion
}