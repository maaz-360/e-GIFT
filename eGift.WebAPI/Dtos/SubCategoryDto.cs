using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;
public record SubCategoryDto(
    int Id,
    [Required] int CategoryId,
    [Required] string SubCategoryName,
    string? Description,
    bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);