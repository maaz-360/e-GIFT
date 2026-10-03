namespace eGift.WebAPI.Models
{
    public class StateModel : BaseModel
    {
        #region Data Model Properties
        public int Id { get; set; }
        public required string StateCode { get; set; }
        public required string StateName { get; set; }
        public int CountryId { get; set; }
        public string? Description { get; set; }
        #endregion
    }
}