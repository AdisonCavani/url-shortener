using ManagementService.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManagementService.Database;

public class AppDbContext : DbContext
{
    public DbSet<DetailsEntity> Details { get; set; } = default!;

    public DbSet<TagEntity> Tags { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DetailsEntity>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<DetailsEntity>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<DetailsEntity>()
            .HasIndex(x => x.UrlId)
            .IsUnique();

        modelBuilder.Entity<DetailsEntity>()
            .Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(36)
            .IsFixedLength();

        modelBuilder.Entity<DetailsEntity>()
            .Property(x => x.Title)
            .HasMaxLength(255);

        modelBuilder.Entity<DetailsEntity>()
            .Property(x => x.Title)
            .IsRequired();

        modelBuilder.Entity<TagEntity>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<TagEntity>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<TagEntity>()
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);
    }
}