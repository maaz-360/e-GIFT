using eGift.WebAPI.Common;
using eGift.WebAPI.Data;
using eGift.WebAPI.Dtos;
using eGift.WebAPI.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eGift.WebAPI.Endpoints;

public static class ProductEndpoint
{
    public static RouteGroupBuilder MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/product").WithTags("Product");

        #region Default CRUD Endpoints

        // GET: api/product
        group.MapGet("/", async (
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ProductEndpoint");

            try
            {
                var products = await (
                    from product in context.Products
                    join category in context.Categories
                        on product.CategoryId equals category.Id
                    join subCategory in context.SubCategories
                        on product.SubCategoryId equals subCategory.Id
                    where !product.IsDeleted
                    select new
                    {
                        Id = product.Id,
                        Name = product.Name,

                        CategoryId = product.CategoryId,
                        CategoryName = category.CategoryName,

                        SubCategoryId = product.SubCategoryId,
                        SubCategoryName = subCategory.SubCategoryName,

                        QuantityPerUnit = product.QuantityPerUnit,
                        UnitPrice = product.UnitPrice,

                        SizeId = product.SizeId,
                        SizeName = product.SizeId.HasValue
                            ? ((Size)product.SizeId.Value).ToString()
                            : null,

                        Discount = product.Discount,
                        UnitInStock = product.UnitInStock,
                        UnitInOrder = product.UnitInOrder,
                        ProductAvailable = product.ProductAvailable,

                        ShortDescription = product.ShortDescription,
                        LongDescription = product.LongDescription,

                        // Additional images
                        PicturePath1 = product.PicturePath1,
                        PicturePath2 = product.PicturePath2,
                        PicturePath3 = product.PicturePath3,
                        PicturePath4 = product.PicturePath4,

                        PictureData1 = product.PictureData1,
                        PictureData2 = product.PictureData2,
                        PictureData3 = product.PictureData3,
                        PictureData4 = product.PictureData4,

                        // Main image
                        ProductImagePath = product.ProductImagePath,
                        ProductImageData = product.ProductImageData,

                        CreatedDate = product.CreatedDate
                    }
                )
                .AsNoTracking()
                .ToListAsync();

                return Results.Ok(products);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in ProductEndpoint: /api/product GET: {Message}.",
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while retrieving products.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // GET: api/product/{id}
        group.MapGet("/{id:int}", async (
            int id,
            AppDBContext context,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ProductEndpoint");

            try
            {
                var product = await (
                    from p in context.Products
                    join category in context.Categories
                        on p.CategoryId equals category.Id
                    join subCategory in context.SubCategories
                        on p.SubCategoryId equals subCategory.Id
                    where p.Id == id && !p.IsDeleted
                    select new
                    {
                        Id = p.Id,
                        Name = p.Name,

                        CategoryId = p.CategoryId,
                        CategoryName = category.CategoryName,

                        SubCategoryId = p.SubCategoryId,
                        SubCategoryName = subCategory.SubCategoryName,

                        QuantityPerUnit = p.QuantityPerUnit,
                        UnitPrice = p.UnitPrice,

                        SizeId = p.SizeId,
                        SizeName = p.SizeId.HasValue
                            ? ((Size)p.SizeId.Value).ToString()
                            : null,

                        Discount = p.Discount,
                        UnitInStock = p.UnitInStock,
                        UnitInOrder = p.UnitInOrder,
                        ProductAvailable = p.ProductAvailable,

                        ShortDescription = p.ShortDescription,
                        LongDescription = p.LongDescription,

                        // Additional images
                        PicturePath1 = p.PicturePath1,
                        PicturePath2 = p.PicturePath2,
                        PicturePath3 = p.PicturePath3,
                        PicturePath4 = p.PicturePath4,

                        PictureData1 = p.PictureData1,
                        PictureData2 = p.PictureData2,
                        PictureData3 = p.PictureData3,
                        PictureData4 = p.PictureData4,

                        // Main image
                        ProductImagePath = p.ProductImagePath,
                        ProductImageData = p.ProductImageData,

                        CreatedDate = p.CreatedDate
                    }
                )
                .AsNoTracking()
                .FirstOrDefaultAsync();

                return product is null
                    ? Results.NotFound()
                    : Results.Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in ProductEndpoint: /api/product/{id} GET: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while retrieving the product with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        // POST: api/product
        group.MapPost("/", async (
            [FromForm] ProductDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment environment) =>
        {
            var logger = loggerFactory.CreateLogger("ProductEndpoint");

            try
            {
                var product = dto.ToEntity();

                var uploadFolder = Path.Combine(
                    environment.ContentRootPath,
                    "uploads",
                    "products");

                Directory.CreateDirectory(uploadFolder);

                // Main Product Image
                if (dto.ProductImage != null &&
                    dto.ProductImage.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.ProductImage.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.ProductImage.CopyToAsync(stream);

                    product.ProductImagePath =
                        $"/uploads/products/{fileName}";
                }

                // Picture 1
                if (dto.Picture1 != null &&
                    dto.Picture1.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.Picture1.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.Picture1.CopyToAsync(stream);

                    product.PicturePath1 =
                        $"/uploads/products/{fileName}";
                }

                // Picture 2
                if (dto.Picture2 != null &&
                    dto.Picture2.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.Picture2.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.Picture2.CopyToAsync(stream);

                    product.PicturePath2 =
                        $"/uploads/products/{fileName}";
                }

                // Picture 3
                if (dto.Picture3 != null &&
                    dto.Picture3.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.Picture3.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.Picture3.CopyToAsync(stream);

                    product.PicturePath3 =
                        $"/uploads/products/{fileName}";
                }

                // Picture 4
                if (dto.Picture4 != null &&
                    dto.Picture4.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.Picture4.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.Picture4.CopyToAsync(stream);

                    product.PicturePath4 =
                        $"/uploads/products/{fileName}";
                }

                context.Products.Add(product);

                await context.SaveChangesAsync();

                return Results.Created(
                    $"/api/product/{product.Id}",
                    product);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in ProductEndpoint: /api/product POST: {Message}.",
                    ex.Message);

                return Results.Json(
                    new
                    {
                        Message = "An error occurred while creating the product.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }).DisableAntiforgery();

        // PUT: api/product/{id}
        group.MapPut("/{id:int}", async (
            int id,
            [FromForm] EditProductDto dto,
            AppDBContext context,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment environment) =>
        {
            var logger = loggerFactory.CreateLogger("ProductEndpoint");

            try
            {
                var existingProduct = await context.Products.FindAsync(id);

                if (existingProduct is null)
                {
                    return Results.NotFound();
                }

                existingProduct.ToEntity(dto);

                var uploadFolder = Path.Combine(
                    environment.ContentRootPath,
                    "uploads",
                    "products");

                Directory.CreateDirectory(uploadFolder);

                // Main Product Image
                if (dto.ProductImage != null &&
                    dto.ProductImage.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.ProductImage.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.ProductImage.CopyToAsync(stream);

                    existingProduct.ProductImagePath =
                        $"/uploads/products/{fileName}";
                }

                // Picture 1
                if (dto.Picture1 != null &&
                    dto.Picture1.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.Picture1.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.Picture1.CopyToAsync(stream);

                    existingProduct.PicturePath1 =
                        $"/uploads/products/{fileName}";
                }

                // Picture 2
                if (dto.Picture2 != null &&
                    dto.Picture2.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.Picture2.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.Picture2.CopyToAsync(stream);

                    existingProduct.PicturePath2 =
                        $"/uploads/products/{fileName}";
                }

                // Picture 3
                if (dto.Picture3 != null &&
                    dto.Picture3.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.Picture3.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.Picture3.CopyToAsync(stream);

                    existingProduct.PicturePath3 =
                        $"/uploads/products/{fileName}";
                }

                // Picture 4
                if (dto.Picture4 != null &&
                    dto.Picture4.Length > 0)
                {
                    var extension = Path.GetExtension(
                        dto.Picture4.FileName);

                    var fileName = $"{Guid.NewGuid()}{extension}";

                    var filePath = Path.Combine(
                        uploadFolder,
                        fileName);

                    await using var stream = new FileStream(
                        filePath,
                        FileMode.Create);

                    await dto.Picture4.CopyToAsync(stream);

                    existingProduct.PicturePath4 =
                        $"/uploads/products/{fileName}";
                }

                context.Products.Update(existingProduct);

                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in ProductEndpoint: /api/product/{id} PUT: {Message}.",
                    id,
                    ex.Message);

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while updating the product with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }).DisableAntiforgery();

        // DELETE: api/product/{id}?loginUserId={loginUserId}&deletedDate={deletedDate}
        group.MapDelete("/{id:int}", async (int id, int loginUserId, DateTime deletedDate, AppDBContext context, ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ProductEndpoint");

            try
            {
                var existingProduct = await context.Products.FindAsync(id);

                if (existingProduct is null)
                {
                    return Results.NotFound();
                }

                existingProduct.IsDeleted = true;
                existingProduct.UpdatedBy = loginUserId;
                existingProduct.UpdatedDate = deletedDate;

                context.Products.Update(existingProduct);
                await context.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    "Exception in ProductEndpoint: /api/product/{id} DELETE: {Message}.",
                    id,
                    ex.Message
                );

                return Results.Json(
                    new
                    {
                        Message = $"An error occurred while deleting the product with ID {id}.",
                        error = ex.Message
                    },
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        });

        #endregion

        #region Image Retrieval

        group.MapGet("/image/{fileName}", async (
            string fileName,
            IWebHostEnvironment environment,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("ProductEndpoint");

            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return Results.NotFound();
                }

                var uploadFolder = Path.Combine(
                    environment.ContentRootPath,
                    "uploads",
                    "products");

                var filePath = Path.Combine(
                    uploadFolder,
                    fileName);

                if (!File.Exists(filePath))
                {
                    return Results.NotFound();
                }

                var extension = Path
                    .GetExtension(fileName)
                    .ToLowerInvariant();

                var contentType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    ".webp" => "image/webp",
                    _ => "application/octet-stream"
                };

                var image = await File.ReadAllBytesAsync(filePath);

                return Results.File(
                    image,
                    contentType);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Exception in ProductEndpoint Image.");

                return Results.NotFound();
            }
        });

        #endregion

        return group;

        
    }

    
}