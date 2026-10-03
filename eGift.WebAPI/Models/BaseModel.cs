namespace eGift.WebAPI.Models
{
    public class BaseModel
    {
        #region Data Model Properties
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        #endregion
    }
}