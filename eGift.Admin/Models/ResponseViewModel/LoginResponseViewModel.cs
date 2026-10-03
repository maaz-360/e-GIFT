using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ResponseViewModel;

public class LoginResponseViewModel
{
    #region View Model Properties
    public string Message { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    #endregion
}