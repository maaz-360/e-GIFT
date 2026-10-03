using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;
public record EditLoginDto(
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
    int UpdatedBy,
    DateTime UpdatedDate
);