namespace eGift.WebAPI.Models
{
    public class CountryModel : BaseModel
    {
        #region Data Model Properties

        public int Id { get; set; }
        public required string CountryCode { get; set; }

        public required string CountryName { get; set; }

        public string? Description { get; set; }

        #endregion
    }
}