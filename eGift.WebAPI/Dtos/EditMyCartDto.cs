using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record EditMyCartDto(
    int Id,
    [Required] int ProductId,
    [Required] int CustomerId,
    [Required] int Quantity,
    bool IsOrdered,
    bool IsDeleted,
    int UpdatedBy,
    DateTime UpdatedDate
);