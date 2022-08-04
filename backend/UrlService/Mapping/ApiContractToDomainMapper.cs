using UrlShortener.Shared.Contracts.Dtos;
using Tag = UrlService.Database.Entities.Tag;

namespace UrlService.Mapping;

public static class ApiContractToDomainMapper
{
    public static Tag ToTagEntity(this TagDto tag)
    {
        return new Tag
        {
            Name = tag.Name
        };
    }

    public static List<Tag> ToTagEntityList(this List<TagDto> tags)
    {
        var tagsList = new List<Tag>();

        foreach (var tag in tags)
            tagsList.Add(tag.ToTagEntity());

        return tagsList;
    }
}