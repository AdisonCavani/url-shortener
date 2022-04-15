using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models;

public class RegisterUserDto
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    
    public DateTime? DateOfBirth { get; set; }

    public int RoleId { get; set; } = 1;
}