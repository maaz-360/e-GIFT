using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class OrderDetailsListViewModel
{
    #region List View Model Properties

    public List<OrderDetailsViewModel> OrderDetailsList { get; set; } = new List<OrderDetailsViewModel>();

    #endregion
}