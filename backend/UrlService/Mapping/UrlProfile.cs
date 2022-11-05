using AutoMapper;
using UrlService.Contracts.Responses;
using UrlService.Database.Entities;

namespace UrlService.Mapping;

public class UrlProfile : Profile
{
    public UrlProfile()
    {
        CreateMap<UrlEntity, GetResponse>()
            .ForMember(x =>
                x.Url, opt =>
                opt.MapFrom(src => src.FullUrl));
    }
}