using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;
public record StateDto(
    int Id,
    [Required] string StateCode,
    [Required] string StateName,
    [Required] int CountryId,
    string? Description,
    bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);