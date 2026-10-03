using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class OrderListViewModel
{
    #region List View Model Properties

    public List<OrderResponseViewModel> OrderList { get; set; } = new List<OrderResponseViewModel>();

    #endregion
}