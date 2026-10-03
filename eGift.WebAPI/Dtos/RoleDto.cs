using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record RoleDto(
    int Id,
    [Required] string RoleName,
    string? Description,
    bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);    