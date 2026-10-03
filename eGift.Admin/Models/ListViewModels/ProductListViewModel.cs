using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class ProductListViewModel
{
    #region List View Model Properties

    public List<ProductResponseViewModel> ProductList { get; set; } = new List<ProductResponseViewModel>();

    #endregion
}