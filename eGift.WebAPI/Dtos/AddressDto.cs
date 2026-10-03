using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public record AddressDto(
    int Id,
    [Required] string Street1,
    string? Street2,
    [Required] int CountryId,
    [Required] int StateId,
    [Required] int CityId,
    [Required] string? PinCode,
    bool IsDeleted,
    int CreatedBy,
    DateTime CreatedDate
);