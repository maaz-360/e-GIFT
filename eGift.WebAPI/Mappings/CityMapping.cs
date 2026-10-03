using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;
public static class CityMapping
{
    // Entity -> DTO
    public static CityDto ToDto(this CityModel entity)
    {
        return new CityDto(
            entity.Id,
            entity.CityCode,
            entity.CityName,
            entity.StateId,
            entity.Description,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }

    // DTO -> Entity (create)
    public static CityModel ToEntity(this CityDto dto)
    {
        return new CityModel
        {
            Id = dto.Id,
            CityCode = dto.CityCode,
            CityName = dto.CityName,
            StateId = dto.StateId,
            Description = dto.Description,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this CityModel entity, EditCityDto dto)
    {
        entity.CityCode = dto.CityCode;
        entity.CityName = dto.CityName;
        entity.StateId = dto.StateId;
        entity.Description = dto.Description;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }
}