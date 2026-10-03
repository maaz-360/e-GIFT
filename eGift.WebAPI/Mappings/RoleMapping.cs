using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class RoleMapping
{
    // Entity -> DTO
    public static RoleDto ToDto(this RoleModel entity)
    {
        return new RoleDto(
            entity.Id,
            entity.RoleName,
            entity.Description,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }

    // DTO -> Entity (create)
    public static RoleModel ToEntity(this RoleDto dto)
    {
        return new RoleModel
        {
            Id = dto.Id,
            RoleName = dto.RoleName,
            Description = dto.Description,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this RoleModel entity, EditRoleDto dto)
    {
        entity.RoleName = dto.RoleName;
        entity.Description = dto.Description;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }
}