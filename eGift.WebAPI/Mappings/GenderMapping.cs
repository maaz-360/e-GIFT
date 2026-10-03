using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class GenderMapping
{
    // Entity -> DTO
    public static GenderDto ToDto(this GenderModel entity)
    {
        return new GenderDto(
           entity.Id,
            entity.GenderName,
            entity.Description,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }


    // DTO -> Entity (create)
    public static GenderModel ToEntity(this GenderDto dto)
    {
        return new GenderModel
        {
            Id = dto.Id,
            GenderName = dto.GenderName,
            Description = dto.Description,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }
    // DTO -> Entity (update existing entity)
    public static void ToEntity(this GenderModel entity, EditGenderDto dto)
    {
        entity.GenderName = dto.GenderName;
        entity.Description = dto.Description;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }


    

       
}

