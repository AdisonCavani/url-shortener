using ManagementService.Contracts.Dtos;

namespace ManagementService.Contracts.Requests;

public class CreateRequest
{
    public string Url { get; set; } = default!;
    
    public string? Title { get; set; }
    
    public List<TagDto>? Tags { get; set; }
}