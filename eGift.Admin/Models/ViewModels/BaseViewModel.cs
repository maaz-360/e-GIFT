namespace eGift.Admin.Models.ViewModels;

public class BaseViewModel
{
    #region Data Model Properties
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    #endregion
}