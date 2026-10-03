using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record EditCityDto(
    int Id,
    [Required] string CityCode,
    [Required] string CityName,
    [Required] int StateId,
    string? Description,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate
);