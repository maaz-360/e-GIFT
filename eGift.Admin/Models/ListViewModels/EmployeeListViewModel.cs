using eGift.Admin.Models.ResponseViewModel;
using eGift.Admin.Models.ViewModels;

namespace eGift.Admin.Models.ListViewModels;

public class EmployeeListViewModel
{
    #region List View Model Properties

    public List<EmployeeResponseViewModel> EmployeeList { get; set; } = new List<EmployeeResponseViewModel>();

    #endregion
}