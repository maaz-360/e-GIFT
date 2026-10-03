using System.ComponentModel.DataAnnotations.Schema;

namespace eGift.WebAPI.Models
{
    public class EmployeeModel : BaseModel
    {
        #region Data Model Properties
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateofBirth { get; set; }
        public int GenderId { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int? AddressId { get; set; }
        public bool IsActive { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ProfileImageData { get; set; }
        public int RoleId { get; set; }
        public bool IsDefault { get; set; }

        #endregion

        #region Not Mapped Properties
        [NotMapped]
        public IFormFile? ProfileImage { get; set; }
        #endregion
    }
}