using Microsoft.EntityFrameworkCore;
using UrlShortener.Entities;

namespace UrlShortener.Data;

/// <summary>
/// Database representation model for app
/// </summary>
public class AppDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Url> Urls { get; set; }

    public virtual DbSet<CustomUrl> CustomUrls { get; set; }


    /// <summary>
    /// Default constructor, expecting database options passed in
    /// </summary>
    /// <param name="options">The database context options</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
