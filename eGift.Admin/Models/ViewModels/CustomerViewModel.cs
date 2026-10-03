using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eGift.Admin.Models.ViewModels;

public class CustomerViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "First Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Date of Birth")]
    [Required(ErrorMessage = "This field is required.")]
    public DateTime? DateofBirth { get; set; }

    [Display(Name = "Gender")]
    [Required(ErrorMessage = "This field is required.")]
    public int GenderId { get; set; }

    [Display(Name = "Mobile")]
    [Required(ErrorMessage = "This field is required.")]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number.")]
    public string Mobile { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "This field is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? Email { get; set; }

    [Display(Name = "Address")]
    public int? AddressId { get; set; }

    [Display(Name = "Is Active")]
    public bool IsActive { get; set; }

    [Display(Name = "Profile Image Path")]
    public string? ProfileImagePath { get; set; }

    [Display(Name = "Profile Image Data")]
    public string? ProfileImageData { get; set; }

    [Display(Name = "Role")]
    [Required(ErrorMessage = "This field is required.")]
    public int RoleId { get; set; }

    [Display(Name = "Is Default")]
    public bool IsDefault { get; set; }

    #endregion

    #region View Model Properties

    [Display(Name = "Age")]
    public int Age { get; set; }

    [Display(Name = "Gender Name")]
    public string? GenderName { get; set; }

    [Display(Name = "Address Name")]
    public string? AddressName { get; set; }

    [Display(Name = "Role Name")]
    public string? RoleName { get; set; }
    public IFormFile? ProfileImage { get; set; }

    [Display(Name = "User Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "Password")]
    [Required(ErrorMessage = "This field is required.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters and contain at least one uppercase letter, one lowercase letter, one number, and one special character."
    )]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = "This field is required.")]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }

    public int LoginId { get; set; }
    #endregion

    #region Select List Properties

    public SelectList? Genders { get; set; }

    public SelectList? Addresses { get; set; }

    public SelectList? Roles { get; set; }

    #endregion
}