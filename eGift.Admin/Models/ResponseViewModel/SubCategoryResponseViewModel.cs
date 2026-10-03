namespace eGift.Admin.Models.ResponseViewModel;

public class SubCategoryResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string SubCategoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    #endregion
}