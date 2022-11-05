namespace ManagementService.Database.Entities;

public class DetailsEntity
{
    public long Id { get; set; }
    
    public long UrlId { get; set; }
    
    public string UserId { get; set; } = default!;
    
    public string? Title { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<TagEntity>? Tags { get; set; }
}