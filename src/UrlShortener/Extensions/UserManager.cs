using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Entities;

namespace UrlShortener.Extensions;

public static class UserManager
{
    public static Task<AppUser?> FindByIdAsync(this UserManager<AppUser> userManager, int userId)
    {
        return userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }
}