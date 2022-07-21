using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Models.Entities;

namespace UrlShortener.Api.Models.App;

public class AppDbContext : DbContext
{
    public virtual DbSet<Url> Urls { get; set; }

    public virtual DbSet<CustomUrl> CustomUrls { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}