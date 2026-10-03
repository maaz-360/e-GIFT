using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record GenderDto(
    int Id,
    [Required] string GenderName,
    string? Description,
    bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);
