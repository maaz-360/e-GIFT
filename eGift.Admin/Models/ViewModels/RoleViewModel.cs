using System.ComponentModel.DataAnnotations;

namespace eGift.Admin.Models.ViewModels;

public class RoleViewModel : BaseViewModel
{
    #region Data Model Properties

    public int Id { get; set; }

    [Display(Name = "Role Name")]
    [Required(ErrorMessage = "This field is required.")]
    public string RoleName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    #endregion
}