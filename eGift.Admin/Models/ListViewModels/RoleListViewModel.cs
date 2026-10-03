using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class RoleListViewModel
{
    #region List View Model Properties

    public List<RoleResponseViewModel> RoleList { get; set; } = new List<RoleResponseViewModel>();

    #endregion
}