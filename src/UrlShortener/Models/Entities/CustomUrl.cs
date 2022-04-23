using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Entities;

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

    public virtual User User { get; set; }
}
