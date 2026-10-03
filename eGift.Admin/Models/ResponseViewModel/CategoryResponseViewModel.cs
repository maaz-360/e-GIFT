namespace eGift.Admin.Models.ResponseViewModel;

public class CategoryResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    #endregion
}