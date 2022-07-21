using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Models.Entities;

public class CustomUrl
{
    [Key]
    [Required]
    [MaxLength(255)]
    public string ShortUrl { get; set; }

    [Required]
    [MaxLength(255)]
    public string FullUrl { get; set; }

    [Required]
    [StringLength(36, MinimumLength = 36, ErrorMessage = "UserId must be UUID v4 format")]
    public string UserId { get; set; }
}
