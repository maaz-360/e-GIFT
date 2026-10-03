using eGift.Store.Models.ViewModels;

namespace eGift.Store.Models.ListViewModels;

public class OrderListViewModel
{
    #region List View Model Properties

    public List<OrderViewModel> OrderList { get; set; } = new List<OrderViewModel>();

    #endregion
}