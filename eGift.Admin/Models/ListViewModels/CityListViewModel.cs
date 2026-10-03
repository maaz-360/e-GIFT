using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class CityListViewModel
{
    #region List View Model Properties

    public List<CityResponseViewModel> CityList { get; set; } = new List<CityResponseViewModel>();

    #endregion
}