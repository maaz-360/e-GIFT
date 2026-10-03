using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;
public record OrderDto(
    int Id,
    [Required] int CustomerId,
    decimal TotalAmount, 
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
    int CreatedBy,
    DateTime CreatedDate,
    List<OrderDetailsDto> Items
);