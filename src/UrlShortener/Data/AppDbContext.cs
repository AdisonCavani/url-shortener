using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Data;

/// <summary>
/// Database representaion model for app
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// The settings for the app
    /// </summary>
    public DbSet<SettingsDataModel> Settings { get; set; }

    /// <summary>
    /// Default constructor, expecting database options passed in
    /// </summary>
    /// <param name="options">The database context options</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
