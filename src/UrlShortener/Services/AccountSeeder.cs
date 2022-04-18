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
        {
            if (!_context.Roles.Any())
            {
                _context.Roles.AddRange(GetAccountRoles());
                _context.SaveChanges();
            }
        }
    }

    private IEnumerable<Role> GetAccountRoles()
    {
        var roles = new List<Role>()
        {
            new Role()
            {
                Name = "User"
            },
            new Role()
            {
                Name = "Admin"
            }
        };

        return roles;
    }
}
