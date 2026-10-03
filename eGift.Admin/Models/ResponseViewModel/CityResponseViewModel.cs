namespace eGift.Admin.Models.ResponseViewModel;

public class CityResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }

    public string CityCode { get; set; } = string.Empty;

    public string CityName { get; set; } = string.Empty;

    public int StateId { get; set; }

    public string StateName { get; set; } = string.Empty;

    public int CountryId { get; set; }

    public string CountryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    #endregion
}