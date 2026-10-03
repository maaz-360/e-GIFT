namespace eGift.Store.Models.ResponseViewModel;

public class MyCartResponseViewModel
{
    #region Response

    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CustomerId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; }
    #endregion

    #region Product Details
    public string ProductName { get; set; } = string.Empty;
    public string? ProductImagePath { get; set; }
    public decimal ProductPrice { get; set; }
    public string? ShortDescription { get; set; }
    #endregion
}