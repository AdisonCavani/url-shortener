using Microsoft.EntityFrameworkCore;
using UrlService.Database.Entities;

namespace UrlService.Database;

public class AppDbContext : DbContext
{
    public virtual DbSet<Url> Urls { get; set; } = default!;
    
    public virtual DbSet<UrlDetails> UrlsDetails { get; set; } = default!;
    
    public virtual DbSet<Tag> Tags { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}