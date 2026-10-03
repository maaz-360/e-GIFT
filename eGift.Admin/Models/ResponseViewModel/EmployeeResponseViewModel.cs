namespace eGift.Admin.Models.ResponseViewModel;

public class EmployeeResponseViewModel
{
    #region View Model Properties
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateofBirth { get; set; }
    public int Age { get; set; }
    public int GenderId { get; set; }
    public string GenderName { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int AddressId { get; set; }
    public string FullAddress { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? ProfileImagePath { get; set; }
    public string? ProfileImageData { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastLogin { get; set; }
    public int LoginId { get; set; }
    #endregion
}