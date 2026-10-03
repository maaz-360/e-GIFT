using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class LoginMapping
{
    // Entity -> DTO
    public static LoginDto ToDto(this LoginModel entity)
    {
        return new LoginDto(
            entity.Id,
            entity.RefId,
            entity.RefType,
            entity.UserName,
            entity.Password,
            entity.RoleId,
            entity.IsActive,
            entity.LogInDate,
            entity.LastLoginDate,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }

    // DTO -> Entity (create)
    public static LoginModel ToEntity(this LoginDto dto)
    {
        return new LoginModel
        {
            Id = dto.Id,
            RefId = dto.RefId,
            RefType = dto.RefType,
            UserName = dto.UserName,
            Password = dto.Password,
            RoleId = dto.RoleId,
            IsActive = dto.IsActive,
            LogInDate = dto.LogInDate,
            LastLoginDate = dto.LastLoginDate,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this LoginModel entity, EditLoginDto dto)
    {
        entity.RefId = dto.RefId;
        entity.RefType = dto.RefType;
        entity.UserName = dto.UserName;
        entity.Password = dto.Password;
        entity.RoleId = dto.RoleId;
        entity.IsActive = dto.IsActive;
        entity.LogInDate = dto.LogInDate;
        entity.LastLoginDate = dto.LastLoginDate;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }
}