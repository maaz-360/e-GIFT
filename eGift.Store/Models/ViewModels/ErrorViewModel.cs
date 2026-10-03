namespace eGift.Store.Models.ViewModels;

public class ErrorViewModel
{
    #region View Model Properties
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    #endregion
}
