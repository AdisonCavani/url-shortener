using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Entities;

namespace UrlShortener.Api.Database;

public class AppDbContext : DbContext
{
    public virtual DbSet<Url> Urls { get; set; } = default!;

    public virtual DbSet<CustomUrl> CustomUrls { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}