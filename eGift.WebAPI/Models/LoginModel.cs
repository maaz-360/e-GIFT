namespace eGift.WebAPI.Models
{
    public class LoginModel : BaseModel
    {
        #region Data Model Properties
        public int Id { get; set; }
        public int RefId { get; set; }
        public required string RefType { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LogInDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        #endregion
    }
}