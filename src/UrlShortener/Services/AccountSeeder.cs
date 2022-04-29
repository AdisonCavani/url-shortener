using UrlShortener.Data;
using UrlShortener.Entities;

namespace UrlShortener.Services;

public class AccountSeeder
{
    private readonly AppDbContext _context;

    public AccountSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (_context.Database.CanConnect())
            if (!_context.Roles.Any())
            {
                _context.Roles.AddRange(GetAccountRoles());
                _context.SaveChanges();
            }
    }

    private static IEnumerable<Role> GetAccountRoles()
    {
        List<Role> roles = new()
        {
            new()
            {
                Name = "User"
            },
            new()
            {
                Name = "Admin"
            }
        };

        return roles;
    }
}
