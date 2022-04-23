using Microsoft.EntityFrameworkCore;
using UrlShortener.Entities;

namespace UrlShortener.Data;

/// <summary>
/// Database representation model for app
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Url> Url { get; set; }

    public DbSet<CustomUrl> CustomUrl { get; set; }


    /// <summary>
    /// Default constructor, expecting database options passed in
    /// </summary>
    /// <param name="options">The database context options</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
