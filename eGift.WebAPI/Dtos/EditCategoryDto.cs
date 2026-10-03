using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record EditCategoryDto(
    int Id,
    [Required] string CategoryName,
    string? Description,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate
);