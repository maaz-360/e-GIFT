using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record CityDto
(
    int Id,
    [Required] string CityCode,
    [Required] string CityName,
    [Required] int StateId,
    string? Description,
     bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);
