using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models.Entities;

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
    public int UserId { get; set; }

    public virtual AppUser User { get; set; }
}
