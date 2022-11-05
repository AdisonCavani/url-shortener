using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace UrlService.UnitTests.Endpoints;

public static class UrlHelpers
{
    public const string USER1_UUID = "4913d21f-390c-4be4-b10b-21611c79db54";
    public const string USER2_UUID = "a8382cef-e9c9-4025-a7ae-1081dc4f0af9";

    public static DbSet<Database.Entities.UrlEntity> GetMockSet()
    {
        var faker = new Faker();

        var id = 1;
        var detailsId = 1;
        var tagId = 1;

        var list = new List<Database.Entities.UrlEntity>();

        for (int i = 1; i <= 100; i++)
        {
            list.Add(new()
            {
                Id = id++,
                FullUrl = faker.Internet.Url(),
                UrlDetails = new()
                {
                    Id = detailsId++,
                    UserId = i <= 50
                        ? USER1_UUID
                        : USER2_UUID,
                    Title = faker.Random.Number(0, 100) <= 20
                        ? faker.Commerce.ProductName()
                        : null,
                    CreatedAt = faker.Date.Past(),

                    Tags = faker.Random.Number(0, 100) <= 20
                        ? new List<TagEntity>
                        {
                            new()
                            {
                                Id = tagId++,
                                Name = faker.Commerce.Department(1)
                            }
                        }
                        : null
                }
            });
        }

        return list.AsQueryable().BuildMockDbSet().Object;
    }

    public static List<Claim> GetClaims()
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, USER1_UUID)
        };

        return claims;
    }

    public static HttpContext GetHttpContext()
    {
        Mock<HttpContext> fakeHttpContext = new();

        GenericIdentity fakeIdentity = new("User");
        fakeIdentity.AddClaims(GetClaims());

        GenericPrincipal principal = new(fakeIdentity, null);

        fakeHttpContext.Setup(t => t.User).Returns(principal);
        return fakeHttpContext.Object;
    }
}