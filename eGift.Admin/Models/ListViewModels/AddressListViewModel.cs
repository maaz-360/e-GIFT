using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class AddressListViewModel
{
    #region List View Model Properties
    public List<AddressResponseViewModel> AddressList { get; set; } = new List<AddressResponseViewModel>();
    #endregion
}