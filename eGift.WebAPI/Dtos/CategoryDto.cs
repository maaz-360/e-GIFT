using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;
public record CategoryDto
(
    int Id,
    [Required] string CategoryName,
    string? Description,
     bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);