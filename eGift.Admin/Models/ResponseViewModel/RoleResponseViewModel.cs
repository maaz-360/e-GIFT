namespace eGift.Admin.Models.ResponseViewModel;

public class RoleResponseViewModel
{
    #region View Model Properties

    public int Id { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    #endregion
}