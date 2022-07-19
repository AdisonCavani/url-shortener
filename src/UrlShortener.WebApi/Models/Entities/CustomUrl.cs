using System.ComponentModel.DataAnnotations;

namespace UrlShortener.WebApi.Models.Entities;

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
    public string UserId { get; set; }
}
