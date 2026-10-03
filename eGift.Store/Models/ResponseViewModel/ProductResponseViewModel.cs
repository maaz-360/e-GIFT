namespace eGift.Store.Models.ResponseViewModel;

public class ProductResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = string.Empty;

    public int QuantityPerUnit { get; set; }

    public decimal UnitPrice { get; set; }

    public int? SizeId { get; set; }

    public string? SizeName { get; set; }

    public decimal? Discount { get; set; }

    public int UnitInStock { get; set; }

    public int UnitInOrder { get; set; }

    public int ProductAvailable { get; set; }

    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    #region Product Images

    // Main Product Image
    public string? ProductImagePath { get; set; }

    // Additional Product Images
    public string? PicturePath1 { get; set; }

    public string? PicturePath2 { get; set; }

    public string? PicturePath3 { get; set; }

    public string? PicturePath4 { get; set; }

    #endregion

    public DateTime CreatedDate { get; set; }

    #endregion
}