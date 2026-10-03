using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class SubCategoryListViewModel
{
    #region List View Model Properties

    public List<SubCategoryResponseViewModel> SubCategoryList { get; set; } = new List<SubCategoryResponseViewModel>();

    #endregion
}