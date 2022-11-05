using Microsoft.EntityFrameworkCore;
using UrlService.Database.Entities;

namespace UrlService.Database;

public class AppDbContext : DbContext
{
    public DbSet<UrlEntity> Urls { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}