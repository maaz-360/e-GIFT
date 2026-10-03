using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class AddressMapping
{
    //Entity -> DTO       
    public static AddressDto ToDto(this AddressModel entity)
    {
        return new AddressDto(
            entity.Id,
            entity.Street1,
            entity.Street2,
            entity.CountryId,
            entity.StateId,
            entity.CityId,
            entity.PinCode,
            entity.IsDeleted,
            entity.CreatedBy,
            entity.CreatedDate
        );
    }

    // DTO -> Entity (create)
    public static AddressModel ToEntity(this AddressDto dto)
    {
        return new AddressModel
        {
            Id = dto.Id,
            Street1 = dto.Street1,
            Street2 = dto.Street2,
            CountryId = dto.CountryId,
            StateId = dto.StateId,
            CityId = dto.CityId,
            PinCode = dto.PinCode,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate
        };
    }

    // DTO -> Entity (update existing entity)

    public static void ToEntity(this AddressModel entity, EditAddressDto dto)
    {
        entity.Street1 = dto.Street1;
        entity.Street2 = dto.Street2;
        entity.CountryId = dto.CountryId;
        entity.StateId = dto.StateId;
        entity.CityId = dto.CityId;
        entity.PinCode = dto.PinCode;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
    }
}