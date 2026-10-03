using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record EditStateDto(
    int Id,
    [Required] string StateCode,
    [Required] string StateName,
    [Required] int CountryId,
    string? Description,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate
);