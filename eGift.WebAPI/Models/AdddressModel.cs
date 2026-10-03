namespace eGift.WebAPI.Models
{
    public class AddressModel : BaseModel
    {
        #region Data Model Properties
        public int Id { get; set; }
        public string Street1 { get; set; } = string.Empty;
        public string? Street2 { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string? PinCode { get; set; }
        #endregion
    }
}