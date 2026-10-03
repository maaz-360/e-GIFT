using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class OrderDetailsMapping
{
    // Entity -> DTO
    public static OrderDetailsDto ToDto(this OrderDetailsModel entity)
    {
        return new OrderDetailsDto(
            entity.Id,
            entity.OrderId,
            entity.ProductId,
            entity.UnitPrice,
            entity.Quantity,
            entity.Discount,
            entity.Tax,
            entity.NetAmount,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }

    // DTO -> Entity (create)
    public static OrderDetailsModel ToEntity(this OrderDetailsDto dto)
    {
        return new OrderDetailsModel
        {
            Id = dto.Id,
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            UnitPrice = dto.UnitPrice,
            Quantity = dto.Quantity,
            Discount = dto.Discount,
            Tax = dto.Tax,
            NetAmount = dto.NetAmount,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this OrderDetailsModel entity, EditOrderDetailsDto dto)
    {
        entity.OrderId = dto.OrderId;
        entity.ProductId = dto.ProductId;
        entity.UnitPrice = dto.UnitPrice;
        entity.Quantity = dto.Quantity;
        entity.Discount = dto.Discount;
        entity.Tax = dto.Tax;
        entity.NetAmount = dto.NetAmount;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }
}