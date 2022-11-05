using AutoMapper;
using ManagementService.Contracts.RabbitMq;
using ManagementService.Database;
using ManagementService.Database.Entities;

namespace ManagementService.Services;

public class DetailsRepository : IDetailsRepository
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public DetailsRepository(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<bool> CreateAsync(UrlCreatedEvent createdEvent)
    {
        var entity = _mapper.Map<DetailsEntity>(createdEvent);
        await _context.Details.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}