using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;
public record EditOrderDto(
    int Id,
    [Required] int CustomerId,
    [Required] decimal TotalAmount,
    decimal? TotalDiscount,
    decimal? TotalTax,
    string OrderNumber,
    string? Notes,
    DateTime? DispatchedDate,
    DateTime? ShippedDate,
    DateTime? DeliveryDate,
    DateTime? CancelDate,
    int StatusId,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate,
    List<OrderDetailsDto> Items
);