using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;
public record EditRoleDto(
    int Id,
    [Required] string RoleName,
    string? Description,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate
);