using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Entities;

public class CustomUrl
{
    [Key]
    [Required]
    [MaxLength(255)]
    public string ShortUrl { get; set; } = default!;

    [Required]
    [MaxLength(255)]
    public string FullUrl { get; set; } = default!;

    [Required]
    [StringLength(36, MinimumLength = 36, ErrorMessage = "UserId must be UUID v4 format")]
    public string UserId { get; set; } = default!;
}