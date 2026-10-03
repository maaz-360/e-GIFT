using System.ComponentModel.DataAnnotations;

namespace eGift.WebAPI.Dtos;

public class CustomerDto
{
   public int Id { get; set; }

   [Required]
   public string FirstName { get; set; } = string.Empty;

   [Required]
   public string LastName { get; set; } = string.Empty;

   [Required]
   public DateTime DateofBirth { get; set; }

   [Required]
   public int GenderId { get; set; }

   [Required]
   public string Mobile { get; set; } = string.Empty;

   public string? Email { get; set; }

   public int? AddressId { get; set; }

   public bool IsActive { get; set; }

   public string? ProfileImagePath { get; set; }

   public string? ProfileImageData { get; set; }

   [Required]
   public int RoleId { get; set; }

   public bool IsDefault { get; set; }

   public bool IsDeleted { get; set; }

   public int CreatedBy { get; set; }

   public DateTime CreatedDate { get; set; }

   public IFormFile? ProfileImage { get; set; }
}