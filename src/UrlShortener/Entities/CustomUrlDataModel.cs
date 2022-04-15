using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Entities;

public class CustomUrlDataModel
{
    /// <summary>
    /// Unique Id for this entry
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    /// <summary>
    /// The settings name
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Name { get; set; }

    /// <summary>
    /// The settings value
    /// </summary>
    [Required]
    [MaxLength(2048)]
    public string Value { get; set; }
}
