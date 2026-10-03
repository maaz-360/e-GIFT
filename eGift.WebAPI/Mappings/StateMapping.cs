using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class StateMapping
{
    // Entity -> DTO
    public static StateDto ToDto(this StateModel entity)
    {
        return new StateDto(
            entity.Id,
            entity.StateCode,
            entity.StateName,
            entity.CountryId,
            entity.Description,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }

    // DTO -> Entity (create)
    public static StateModel ToEntity(this StateDto dto)
    {
        return new StateModel
        {
            Id = dto.Id,
            StateCode = dto.StateCode,
            StateName = dto.StateName,
            CountryId = dto.CountryId,
            Description = dto.Description,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this StateModel entity, EditStateDto dto)
    {
        entity.StateCode = dto.StateCode;
        entity.StateName = dto.StateName;
        entity.CountryId = dto.CountryId;
        entity.Description = dto.Description;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }
}