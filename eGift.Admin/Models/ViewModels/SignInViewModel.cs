using System.ComponentModel.DataAnnotations;

namespace eGift.Admin.Models.ViewModels;

public class SignInViewModel
{
    #region View Model Properties
    [Display(Name ="User Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "Password")]
    [Required(ErrorMessage = "This field is required.")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
    #endregion
}