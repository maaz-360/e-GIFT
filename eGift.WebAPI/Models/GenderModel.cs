namespace eGift.WebAPI.Models
{
    public class GenderModel : BaseModel
    {
        #region Data Model Properties

        public int Id { get; set; }

        public required string GenderName { get; set; }

        public string? Description { get; set; }

        #endregion
    }
}