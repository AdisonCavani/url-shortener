using HashidsNet;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.Api.Database;
using UrlShortener.Api.Endpoints.UserUrl;

namespace UrlShortener.UnitTests.Endpoints.UserUrl;

public class SaveTests
{
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    
    private readonly Save _endpoint;
    
    public SaveTests()
    {
        _endpoint = new(_context.Object, new Hashids("1234", 7));
    }
}