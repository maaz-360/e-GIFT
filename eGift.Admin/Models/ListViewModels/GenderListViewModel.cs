using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class GenderListViewModel
{
    #region List View Model Properties

    public List<GenderResponseViewModel> GenderList { get; set; } = new List<GenderResponseViewModel>();

    #endregion
}