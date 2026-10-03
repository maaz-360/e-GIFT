namespace eGift.Admin.Models.ResponseViewModel;

public class CountryResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }

    public string CountryCode { get; set; } = string.Empty;

    public string CountryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    #endregion
}