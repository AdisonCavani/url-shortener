using ProtoBuf;

namespace UrlService.Contracts.Dtos;

[ProtoContract]
public class TagDto
{
    [ProtoMember(1)]
    public string Name { get; set; } = default!;
}