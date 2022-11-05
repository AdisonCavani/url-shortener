using ManagementService.Contracts.Dtos;
using ProtoBuf;

namespace ManagementService.Contracts.RabbitMq;

[ProtoContract]
public class UrlCreatedEvent
{
    [ProtoMember(1)]
    public long DetailsId { get; set; }

    public string Url { get; set; } = default!;
}