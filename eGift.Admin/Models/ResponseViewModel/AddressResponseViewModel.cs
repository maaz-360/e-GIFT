namespace eGift.Admin.Models.ResponseViewModel;

public class AddressResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }

    public string Street1 { get; set; } = string.Empty;

    public string? Street2 { get; set; }

    public int CityId { get; set; }

    public int StateId { get; set; }

    public int CountryId { get; set; }

    public string? PinCode { get; set; }

    public string CountryName { get; set; } = string.Empty;

    public string StateName { get; set; } = string.Empty;

    public string CityName { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public string FullAddress { get; set; } = string.Empty;

    #endregion
}