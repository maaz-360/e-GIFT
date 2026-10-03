namespace eGift.WebAPI.Models
{
    public class SubCategoryModel : BaseModel
    {
        #region Data Model Properties

        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string SubCategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        #endregion
    }
}          