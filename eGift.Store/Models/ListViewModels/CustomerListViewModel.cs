using eGift.Store.Models.ViewModels;

namespace eGift.Store.Models.ListViewModels;

public class CustomerListViewModel
{
    #region List View Model Properties

    public List<CustomerViewModel> CustomerList { get; set; } = new List<CustomerViewModel>();

    #endregion
}