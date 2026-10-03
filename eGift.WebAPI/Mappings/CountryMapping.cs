using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class CountryMapping
{
    // Entity -> DTO
    public static CountryDto ToDto(this CountryModel entity)
    {
        return new CountryDto(
            entity.Id,
            entity.CountryCode,
            entity.CountryName,
            entity.Description,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }

    // DTO -> Entity (create)
    public static CountryModel ToEntity(this CountryDto dto)
    {
        return new CountryModel
        {
            Id = dto.Id,
            CountryCode = dto.CountryCode,
            CountryName = dto.CountryName,
            Description = dto.Description,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this CountryModel entity, EditCountryDto dto)
    {
        entity.CountryCode = dto.CountryCode;
        entity.CountryName = dto.CountryName;
        entity.Description = dto.Description;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }
}