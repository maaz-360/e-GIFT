using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class CategoryListViewModel
{
    #region List View Model Properties

    public List<CategoryResponseViewModel> CategoryList { get; set; } = new List<CategoryResponseViewModel>();

    #endregion
}