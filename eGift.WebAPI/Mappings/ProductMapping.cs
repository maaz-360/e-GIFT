using eGift.WebAPI.Dtos;
using eGift.WebAPI.Models;

namespace eGift.WebAPI.Mappings;

public static class ProductMapping
{
    // Entity -> DTO
    public static ProductDto ToDto(this ProductModel entity)
    {
        return new ProductDto
        {
            Id = entity.Id,

            Name = entity.Name,
            CategoryId = entity.CategoryId,
            SubCategoryId = entity.SubCategoryId,
            QuantityPerUnit = entity.QuantityPerUnit,
            UnitPrice = entity.UnitPrice,
            SizeId = entity.SizeId,
            Discount = entity.Discount,
            UnitInStock = entity.UnitInStock,
            UnitInOrder = entity.UnitInOrder,
            ProductAvailable = entity.ProductAvailable,

            ShortDescription = entity.ShortDescription,
            LongDescription = entity.LongDescription,

            // Additional images
            PicturePath1 = entity.PicturePath1,
            PicturePath2 = entity.PicturePath2,
            PicturePath3 = entity.PicturePath3,
            PicturePath4 = entity.PicturePath4,

            PictureData1 = entity.PictureData1,
            PictureData2 = entity.PictureData2,
            PictureData3 = entity.PictureData3,
            PictureData4 = entity.PictureData4,

            // Main image
            ProductImagePath = entity.ProductImagePath,
            ProductImageData = entity.ProductImageData,

            IsDeleted = entity.IsDeleted,
            CreatedBy = entity.CreatedBy,
            CreatedDate = entity.CreatedDate,

            // Uploaded files
            ProductImage = entity.ProductImage,
            Picture1 = entity.Picture1,
            Picture2 = entity.Picture2,
            Picture3 = entity.Picture3,
            Picture4 = entity.Picture4
        };
    }

    // DTO -> Entity (create)
    public static ProductModel ToEntity(this ProductDto dto)
    {
        return new ProductModel
        {
            Id = dto.Id,

            Name = dto.Name,
            CategoryId = dto.CategoryId,
            SubCategoryId = dto.SubCategoryId,
            QuantityPerUnit = dto.QuantityPerUnit,
            UnitPrice = dto.UnitPrice,
            SizeId = dto.SizeId,
            Discount = dto.Discount,
            UnitInStock = dto.UnitInStock,
            UnitInOrder = dto.UnitInOrder,
            ProductAvailable = dto.ProductAvailable,

            ShortDescription = dto.ShortDescription,
            LongDescription = dto.LongDescription,

            // Additional images
            PicturePath1 = dto.PicturePath1,
            PicturePath2 = dto.PicturePath2,
            PicturePath3 = dto.PicturePath3,
            PicturePath4 = dto.PicturePath4,

            PictureData1 = dto.PictureData1,
            PictureData2 = dto.PictureData2,
            PictureData3 = dto.PictureData3,
            PictureData4 = dto.PictureData4,

            // Main image
            ProductImagePath = dto.ProductImagePath,
            ProductImageData = dto.ProductImageData,

            IsDeleted = dto.IsDeleted,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate,

            // Uploaded files
            ProductImage = dto.ProductImage,
            Picture1 = dto.Picture1,
            Picture2 = dto.Picture2,
            Picture3 = dto.Picture3,
            Picture4 = dto.Picture4
        };
    }

    // DTO -> Entity (update existing entity)
    public static void ToEntity(this ProductModel entity, EditProductDto dto)
    {
        entity.Name = dto.Name;
        entity.CategoryId = dto.CategoryId;
        entity.SubCategoryId = dto.SubCategoryId;
        entity.QuantityPerUnit = dto.QuantityPerUnit;
        entity.UnitPrice = dto.UnitPrice;
        entity.SizeId = dto.SizeId;
        entity.Discount = dto.Discount;
        entity.UnitInStock = dto.UnitInStock;
        entity.UnitInOrder = dto.UnitInOrder;
        entity.ProductAvailable = dto.ProductAvailable;

        entity.ShortDescription = dto.ShortDescription;
        entity.LongDescription = dto.LongDescription;

        // Additional images
        entity.PicturePath1 = dto.PicturePath1;
        entity.PicturePath2 = dto.PicturePath2;
        entity.PicturePath3 = dto.PicturePath3;
        entity.PicturePath4 = dto.PicturePath4;

        entity.PictureData1 = dto.PictureData1;
        entity.PictureData2 = dto.PictureData2;
        entity.PictureData3 = dto.PictureData3;
        entity.PictureData4 = dto.PictureData4;

        // Main image
        entity.ProductImagePath = dto.ProductImagePath;
        entity.ProductImageData = dto.ProductImageData;

        entity.IsDeleted = dto.IsDeleted;
        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedDate = dto.UpdatedDate;

        // Uploaded files
        entity.ProductImage = dto.ProductImage;
        entity.Picture1 = dto.Picture1;
        entity.Picture2 = dto.Picture2;
        entity.Picture3 = dto.Picture3;
        entity.Picture4 = dto.Picture4;
    }
}