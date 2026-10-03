using eGift.Store.Models.ViewModels;

namespace eGift.Store.Models.ListViewModels;

public class OrderDetailsListViewModel
{
    #region List View Model Properties

    public List<OrderDetailsViewModel> OrderDetailsList { get; set; } = new List<OrderDetailsViewModel>();

    #endregion
}