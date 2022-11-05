using AutoMapper;
using ManagementService.Contracts.Dtos;
using ManagementService.Database.Entities;

namespace ManagementService.Mapping;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<TagDto, TagEntity>();
    }
}