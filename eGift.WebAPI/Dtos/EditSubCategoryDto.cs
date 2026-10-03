using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;
public record EditSubCategoryDto(
    int Id,
    [Required] int CategoryId,
    [Required] string SubCategoryName,
    string? Description,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate
);