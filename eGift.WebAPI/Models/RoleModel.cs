namespace eGift.WebAPI.Models
{
    public class RoleModel : BaseModel
    {
        #region Data Model Properties

        public int Id { get; set; }

        public required string RoleName { get; set; }

        public string? Description { get; set; }

        #endregion
    }
}