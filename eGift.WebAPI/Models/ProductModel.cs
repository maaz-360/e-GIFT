using System.ComponentModel.DataAnnotations.Schema;

namespace eGift.WebAPI.Models;

public class ProductModel : BaseModel
{
  
    #region Data Model Properties

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public int SubCategoryId { get; set; }

    public int QuantityPerUnit { get; set; }

    public decimal UnitPrice { get; set; }

    public int? SizeId { get; set; }

    public decimal? Discount { get; set; }

    public int UnitInStock { get; set; }

    public int UnitInOrder { get; set; }

    public int ProductAvailable { get; set; }

    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    // Additional Product Images
    public string? PicturePath1 { get; set; }
    public string? PicturePath2 { get; set; }
    public string? PicturePath3 { get; set; }
    public string? PicturePath4 { get; set; }
    
    public string? PictureData1 { get; set; }
    public string? PictureData2 { get; set; }
    public string? PictureData3 { get; set; }
    public string? PictureData4 { get; set; }   
   // Main Product Image
    public string? ProductImagePath { get; set; }
    public string? ProductImageData { get; set; }
  
    #endregion


    #region Not Mapped Properties

    [NotMapped]
    public IFormFile? ProductImage { get; set; }

    [NotMapped]
    public IFormFile? Picture1 { get; set; }

    [NotMapped]
    public IFormFile? Picture2 { get; set; }

    [NotMapped]
    public IFormFile? Picture3 { get; set; }

    [NotMapped]
    public IFormFile? Picture4 { get; set; }

    #endregion
}