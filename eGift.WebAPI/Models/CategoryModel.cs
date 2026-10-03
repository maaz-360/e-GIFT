namespace eGift.WebAPI.Models
{
    public class CategoryModel : BaseModel
    {
        #region Data Model Properties
        public int Id { get; set; }
        public required string CategoryName { get; set; }
        public string? Description { get; set; }
        #endregion
    }
}