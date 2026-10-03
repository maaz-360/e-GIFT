using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class OrderMapping
{
    // Entity -> DTO
    public static OrderDto ToDto(this OrderModel entity)
    {
        return new OrderDto(
            entity.Id,
            entity.CustomerId,
            entity.TotalAmount,
            entity.TotalDiscount,
            entity.TotalTax,
            entity.OrderNumber,
            entity.Notes,
            entity.DispatchedDate,
            entity.ShippedDate,
            entity.DeliveryDate,
            entity.CancelDate,
            entity.StatusId,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate,
            entity.Items
            .Select(x => x.ToDto())
            .ToList()
        );
    }

    // DTO -> Entity (create)
    public static OrderModel ToEntity(this OrderDto dto)
    {
        return new OrderModel
        {
            Id = dto.Id,
            CustomerId = dto.CustomerId,
            TotalAmount = dto.TotalAmount,
            TotalDiscount = dto.TotalDiscount,
            TotalTax = dto.TotalTax,
            OrderNumber = dto.OrderNumber,
            Notes = dto.Notes,
            DispatchedDate = dto.DispatchedDate,
            ShippedDate = dto.ShippedDate,
            DeliveryDate = dto.DeliveryDate,
            CancelDate = dto.CancelDate,
            StatusId = dto.StatusId,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate,
            Items = dto.Items.Select(x=>x.ToEntity()).ToList()
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this OrderModel entity, EditOrderDto dto)
    {
        entity.CustomerId = dto.CustomerId;
        entity.TotalAmount = dto.TotalAmount;
        entity.TotalDiscount = dto.TotalDiscount;
        entity.TotalTax = dto.TotalTax;
        entity.OrderNumber = dto.OrderNumber;
        entity.Notes = dto.Notes;
        entity.DispatchedDate = dto.DispatchedDate;
        entity.ShippedDate = dto.ShippedDate;
        entity.DeliveryDate = dto.DeliveryDate;
        entity.CancelDate = dto.CancelDate;
        entity.StatusId = dto.StatusId;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
        entity.Items = dto.Items.Select(x => x.ToEntity()).ToList();
    }
}