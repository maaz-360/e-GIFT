namespace eGift.Store.Models.ResponseViewModel;

public class OrderResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? TotalDiscount { get; set; }
    public decimal? TotalTax { get; set; }
    public string? OrderNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime? DispatchedDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? CancelDate { get; set; }
    public int StatusId { get; set; }
    public DateTime CreatedDate { get; set; }
    
    #endregion
}