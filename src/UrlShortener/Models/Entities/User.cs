using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Models.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string PasswordHash { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; }
}