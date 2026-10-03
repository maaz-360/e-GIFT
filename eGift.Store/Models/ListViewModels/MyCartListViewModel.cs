
using eGift.Store.Models.ResponseViewModel;

namespace eGift.Store.Models.ListViewModels;

public class MyCartListViewModel
{
    #region List View Model Properties
    public List<MyCartResponseViewModel> MyCartList { get; set; }=new List<MyCartResponseViewModel>();
    #endregion
}