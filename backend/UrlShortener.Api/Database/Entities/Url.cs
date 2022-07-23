using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Api.Database.Entities;

public class Url
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string FullUrl { get; set; } = default!;

    [Required]
    [StringLength(36, MinimumLength = 36, ErrorMessage = $"{nameof(UserId)} must be in UUID v4 format")]
    public string UserId { get; set; } = default!;
}