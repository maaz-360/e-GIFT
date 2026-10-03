using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class SubCategoryMapping
{
    // Entity -> DTO
    public static SubCategoryDto ToDto(this SubCategoryModel entity)
    {
        return new SubCategoryDto(
            entity.Id,
            entity.CategoryId,
            entity.SubCategoryName,
            entity.Description,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }

    // DTO -> Entity (create)
    public static SubCategoryModel ToEntity(this SubCategoryDto dto)
    {
        return new SubCategoryModel
        {
            Id = dto.Id,
            CategoryId = dto.CategoryId,
            SubCategoryName = dto.SubCategoryName,
            Description = dto.Description,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this SubCategoryModel entity, EditSubCategoryDto dto)
    {
        entity.CategoryId = dto.CategoryId;
        entity.SubCategoryName = dto.SubCategoryName;
        entity.Description = dto.Description;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }
}