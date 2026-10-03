using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record MyCartDto(
    int Id,
    [Required] int ProductId,
    [Required] int CustomerId,
    [Required] int Quantity,
    bool IsOrdered,
    bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);