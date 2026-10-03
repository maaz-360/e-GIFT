using eGift.Store.Models.ViewModels;

namespace eGift.Store.Models.ListViewModels;

public class LoginListViewModel
{
    #region List View Model Properties

    public List<LoginViewModel> LoginList { get; set; } = new List<LoginViewModel>();

    #endregion
}