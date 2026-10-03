using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class LoginListViewModel
{
    #region List View Model Properties

    public List<LoginViewModel> LoginList { get; set; } = new List<LoginViewModel>();

    #endregion
}