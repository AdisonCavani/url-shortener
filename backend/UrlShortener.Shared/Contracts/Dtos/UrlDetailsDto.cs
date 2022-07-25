namespace UrlShortener.Shared.Contracts.Dtos;

public class UrlDetailsDto
{
    public string? Title { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public List<TagDto>? Tags { get; set; }
}