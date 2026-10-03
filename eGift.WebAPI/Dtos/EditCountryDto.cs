using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record EditCountryDto(
    int Id,
    [Required] string CountryCode,
    [Required] string CountryName,
    string? Description,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate
);