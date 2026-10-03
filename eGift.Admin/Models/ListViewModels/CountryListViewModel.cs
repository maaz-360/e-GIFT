using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class CountryListViewModel
{
    #region List View Model Properties

    public List<CountryResponseViewModel> CountryList { get; set; } = new List<CountryResponseViewModel>();

    #endregion
}