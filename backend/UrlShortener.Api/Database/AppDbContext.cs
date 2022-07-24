using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Database.Entities;

namespace UrlShortener.Api.Database;

public class AppDbContext : DbContext
{
    public virtual DbSet<Url> Urls { get; set; } = default!;

    public virtual DbSet<UserUrl> UserUrls { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}