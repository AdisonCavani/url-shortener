using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlService.Database.Entities;

public class Url
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string FullUrl { get; set; } = default!;

    public UrlDetails? UrlDetails { get; set; }
}