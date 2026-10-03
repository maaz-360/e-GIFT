using System.ComponentModel.DataAnnotations;

namespace  eGift.WebAPI.Dtos;

public record CountryDto(
    int Id,
    [Required] string CountryCode,
    [Required] string CountryName,
    string? Description,
    bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);