using ProtoBuf;
using UrlService.Contracts.Dtos;

namespace UrlService.Contracts.RabbitMq;

[ProtoContract]
public class UrlCreatedEvent
{
    [ProtoMember(1)]
    public long UrlId { get; set; }

    [ProtoMember(2)]
    public string UserId { get; set; } = default!;
    
    [ProtoMember(3)]
    public string? Title { get; set; }

    [ProtoMember(4)]
    public List<TagDto>? Tags { get; set; }
}