using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;
public static class CategoryMapping
{
    // Entity -> DTO 
    public static CategoryDto ToDto(this CategoryModel entity)
    {
        return new CategoryDto(
            entity.Id,
            entity.CategoryName,
            entity.Description,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }
        
    // DTO -> Entity (create)
    public static CategoryModel ToEntity(this CategoryDto dto)
    {
        return new CategoryModel
        {
            Id = dto.Id,
            CategoryName = dto.CategoryName,
            Description = dto.Description,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy, 
            CreatedDate = dto.CreatedDate 
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this CategoryModel entity, EditCategoryDto dto)
    {        
        entity.CategoryName = dto.CategoryName;
        entity.Description = dto.Description;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy; 
        entity.UpdatedDate = dto.UpdatedDate;         
    }
}