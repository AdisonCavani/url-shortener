using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Entities;

/// <summary>
/// Settings database table representational model
/// </summary>
public class SettingsDataModel
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