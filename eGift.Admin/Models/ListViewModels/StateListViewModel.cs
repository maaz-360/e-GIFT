using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class StateListViewModel
{
    #region List View Model Properties

    public List<StateResponseViewModel> StateList { get; set; } = new List<StateResponseViewModel>();

    #endregion
}