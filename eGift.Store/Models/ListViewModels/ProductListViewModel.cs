using eGift.Store.Models.ViewModels;

namespace eGift.Store.Models.ListViewModels;

public class ProductListViewModel
{
    #region List View Model Properties

    public List<ProductViewModel> ProductList { get; set; } = new List<ProductViewModel>();

    #endregion
}