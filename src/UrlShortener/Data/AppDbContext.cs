using Microsoft.EntityFrameworkCore;
using UrlShortener.Entities;

namespace UrlShortener.Data;

/// <summary>
/// Database representaion model for app
/// </summary>
public class AppDbContext : DbContext
{
    public DbSet<UrlDataModel> Url { get; set; }

    /// <summary>
    /// Default constructor, expecting database options passed in
    /// </summary>
    /// <param name="options">The database context options</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
}
