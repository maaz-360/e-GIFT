using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public class ProductDto
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int SubCategoryId { get; set; }

    [Required]
    public int QuantityPerUnit { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public int? SizeId { get; set; }

    [Required]
    public decimal? Discount { get; set; }

    [Required]
    public int UnitInStock { get; set; }

    [Required]
    public int UnitInOrder { get; set; }

    [Required]
    public int ProductAvailable { get; set; }

    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    public string? PicturePath1 { get; set; }

    public string? PicturePath2 { get; set; }

    public string? PicturePath3 { get; set; }

    public string? PicturePath4 { get; set; }

    public string? PictureData1 { get; set; }

    public string? PictureData2 { get; set; }

    public string? PictureData3 { get; set; }

    public string? PictureData4 { get; set; }

    public string? ProductImagePath { get; set; }

    public string? ProductImageData { get; set; }

    public IFormFile? ProductImage { get; set; }

    public IFormFile? Picture1 { get; set; }

    public IFormFile? Picture2 { get; set; }

    public IFormFile? Picture3 { get; set; }

    public IFormFile? Picture4 { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }


}