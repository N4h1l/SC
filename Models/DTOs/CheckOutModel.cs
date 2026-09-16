using System.ComponentModel.DataAnnotations;

namespace SC.Models.DTOs;

public class CheckOutModel
{
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required] 
    public string? MobileNumber { get; set; }
    [Required]
    [MaxLength(200)]
    public string? Address { get; set; }
    [Required]
    public string? PaymentMethod { get; set; }

}
