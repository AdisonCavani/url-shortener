using AutoMapper;
using UrlService.Database.Entities;
using UrlShortener.Shared.Contracts.Dtos;

namespace UrlService.Mapping;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<TagDto, Tag>();
        CreateMap<Tag, TagDto>();
        CreateMap<UrlDetails, UrlDetailsDto>();
    }
}