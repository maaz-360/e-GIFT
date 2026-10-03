using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;
public record EditGenderDto(
    int Id,
    [Required] string GenderName,
    string? Description,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate
);