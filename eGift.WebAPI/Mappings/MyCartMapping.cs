using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class MyCartMapping
{
    // Entity -> DTO
    public static MyCartDto toDto(this MyCartModel entity)
    {
        return new MyCartDto(
            entity.Id,
            entity.ProductId,
            entity.CustomerId,
            entity.Quantity,
            entity.IsOrdered,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }

    // DTO -> Entity (create)
    public static MyCartModel ToEntity(this MyCartDto dto)
    {
        return new MyCartModel
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            CustomerId = dto.CustomerId,
            Quantity = dto.Quantity,
            IsOrdered = dto.IsOrdered,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this MyCartModel entity, EditMyCartDto dto)
    {
        entity.ProductId = dto.ProductId;
        entity.CustomerId = dto.CustomerId;
        entity.Quantity = dto.Quantity;
        entity.IsOrdered = dto.IsOrdered;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }
}