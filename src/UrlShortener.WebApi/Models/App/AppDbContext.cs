using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.WebApi.Models.Entities;

namespace UrlShortener.WebApi.Models.App;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public virtual DbSet<Url> Urls { get; set; }

    public virtual DbSet<CustomUrl> CustomUrls { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppRole>().HasData(new List<AppRole>
        {
            new()
            {
                Id = 1,
                Name = "User"
            },
            new()
            {
                Id = 2,
                Name = "Admin"
            }
        });
    }
}