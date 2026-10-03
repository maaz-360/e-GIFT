namespace eGift.WebAPI.Models;

public class CityModel : BaseModel
{
    #region Data Model Properties
    public int Id { get; set; }
    public required string CityCode { get; set; }
    public required string CityName { get; set; }
    public int StateId { get; set; }
    public string? Description { get; set; }
    #endregion
}
