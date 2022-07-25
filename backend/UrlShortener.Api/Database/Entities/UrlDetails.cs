using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Api.Database.Entities;

public class UrlDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(36, MinimumLength = 36, ErrorMessage = $"{nameof(UserId)} must be in UUID v4 format")]
    public string UserId { get; set; } = default!;
    
    [MaxLength(255)]
    public string? Title { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Tag>? Tags { get; set; }
}