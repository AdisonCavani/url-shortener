using AutoMapper;
using ManagementService.Contracts.RabbitMq;
using ManagementService.Database.Entities;

namespace ManagementService.Mapping;

public class DetailsProfile : Profile
{
    public DetailsProfile()
    {
        CreateMap<UrlCreatedEvent, DetailsEntity>();
    }
}