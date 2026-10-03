using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;
public static class CustomerMapping
{
    // Entity -> DTO
    public static CustomerDto ToDto(this CustomerModel entity)
    {
        return new CustomerDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            DateofBirth = entity.DateofBirth,
            GenderId = entity.GenderId,
            Mobile = entity.Mobile,
            Email = entity.Email,
            AddressId = entity.AddressId,
            IsActive = entity.IsActive,
            ProfileImagePath = entity.ProfileImagePath,
            ProfileImageData = entity.ProfileImageData,
            RoleId = entity.RoleId,
            IsDefault = entity.IsDefault,
            IsDeleted = entity.IsDeleted,
            CreatedBy = entity.CreatedBy,
            CreatedDate = entity.CreatedDate,
            ProfileImage = entity.ProfileImage
        };
    }

    // DTO -> Entity (create)
    public static CustomerModel ToEntity(this CustomerDto dto)
    {
        return new CustomerModel
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateofBirth = dto.DateofBirth,
            GenderId = dto.GenderId,
            Mobile = dto.Mobile,
            Email = dto.Email,
            AddressId = dto.AddressId,
            IsActive = dto.IsActive,
            ProfileImagePath = dto.ProfileImagePath,
            ProfileImageData = dto.ProfileImageData,
            RoleId = dto.RoleId,
            IsDefault = dto.IsDefault,
            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate,
            ProfileImage = dto.ProfileImage
        };
    }
    // DTO -> Entity (update existing entity)
    public static void ToEntity(this CustomerModel entity, EditCustomerDto dto)
    {
        entity.FirstName = dto.FirstName;
        entity.LastName = dto.LastName;
        entity.DateofBirth = dto.DateofBirth;
        entity.GenderId = dto.GenderId;
        entity.Mobile = dto.Mobile;
        entity.Email = dto.Email;
        entity.AddressId = dto.AddressId;
        entity.IsActive = dto.IsActive;
        entity.RoleId = dto.RoleId;
        entity.IsDefault = dto.IsDefault;
        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;
        entity.ProfileImage = dto.ProfileImage;

    }

}