using System.ComponentModel.DataAnnotations;

namespace eGift.Admin.Models.ViewModels;

public class LoginViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Reference Id")]
    [Required(ErrorMessage = "This field is required.")]
    public int RefId { get; set; }

    [Display(Name = "Reference Type")]
    [Required(ErrorMessage = "This field is required.")]
    public string RefType { get; set; } = string.Empty;

    [Display(Name = "User Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "Password")]
    [Required(ErrorMessage = "This field is required.")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Role")]
    [Required(ErrorMessage = "This field is required.")]
    public int RoleId { get; set; }

    [Display(Name = "Is Active")]
    [Required(ErrorMessage = "This field is required.")]
    public bool IsActive { get; set; }

    [Display(Name = "Log In Date")]
    public DateTime? LogInDate { get; set; }

    [Display(Name = "Last Login Date")]
    public DateTime? LastLoginDate { get; set; }

    #endregion

    #region View Model Properties

    [Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }
    
    #endregion
}