using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record OrderDetailsDto(
    int Id,
    int OrderId,
    [Required] int ProductId,
    decimal UnitPrice,
    [Required] int Quantity,
    decimal? Discount,
    decimal? Tax,
    decimal NetAmount,
    bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);