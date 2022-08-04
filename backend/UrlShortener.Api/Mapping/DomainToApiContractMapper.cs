using UrlShortener.Api.Database.Entities;
using UrlShortener.Shared.Contracts.Dtos;

namespace UrlShortener.Api.Mapping;

public static class DomainToApiContractMapper
{
    public static List<TagDto> ToTagList(this List<Tag> tags)
    {
        var list = new List<TagDto>();

        foreach (var tag in tags)
            list.Add(tag.ToTag());

        return list;
    }
    
    public static TagDto ToTag(this Tag tag)
    {
        return new TagDto
        {
            Name = tag.Name
        };
    }
    
    public static UrlDetailsDto ToUrlDetails(this UrlDetails urlDetails)
    {
        return new UrlDetailsDto
        {
            CreatedAt = urlDetails.CreatedAt,
            Title = urlDetails.Title,
            Tags = urlDetails.Tags?.ToTagList()
        };
    }
}