using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record EditOrderDetailsDto(
    int Id,
    [Required] int OrderId,
    [Required] int ProductId,
    [Required] decimal UnitPrice,
    [Required] int Quantity,
    decimal? Discount,
    decimal? Tax,
    decimal NetAmount,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate
);