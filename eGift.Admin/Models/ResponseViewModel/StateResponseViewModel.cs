namespace eGift.Admin.Models.ResponseViewModel;

public class StateResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }

    public string StateCode { get; set; } = string.Empty;

    public string StateName { get; set; } = string.Empty;

    public int CountryId { get; set; }

    public string CountryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    #endregion
}