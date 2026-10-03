using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record LoginDto(
    int Id,
    [Required] int RefId,
    [Required] string RefType,
    [Required] string UserName,
    [Required] string Password,
    [Required] int RoleId,
    [Required] bool IsActive,
    DateTime? LogInDate,
    DateTime? LastLoginDate,
    bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);
