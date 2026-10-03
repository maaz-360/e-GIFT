namespace eGift.Admin.Models.ResponseViewModel;

public class OrderDetailsResponseViewModel
{
    #region Response View Model Properties

    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Tax { get; set; }
    public decimal NetAmount { get; set; }

    public string ProductName { get; set; } = string.Empty;


    #endregion

}