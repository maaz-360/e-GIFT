using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class CustomerListViewModel
{
    #region List View Model Properties

    public List<CustomerResponseViewModel> CustomerList { get; set; } = new List<CustomerResponseViewModel>();

    #endregion
}