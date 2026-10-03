namespace eGift.Admin.Models.ResponseViewModel;

public class GenderResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }

    public string GenderName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    #endregion
}