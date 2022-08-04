using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HashidsNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UrlShortener.Api.Database;
using UrlShortener.Api.Endpoints.UserUrl;
using UrlShortener.Shared.Contracts.Requests;
using UrlShortener.Shared.Contracts.Responses;
using Xunit;

namespace UrlShortener.UnitTests.Endpoints.UserUrl;

public class GetAllTests
{
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    
    private readonly GetAll _endpoint;
    
    public GetAllTests()
    {
        _context.Setup(m => m.Urls).Returns(UrlHelpers.GetMockSet());
        
        _endpoint = new(_context.Object, new Hashids("1234", 7));
    }

    [Fact]
    public async Task WhenInvalidClaims_Returns500()
    {
        // Arrange
        var req = new GetAllUserUrlsRequest
        {
            Page = 1
        };
        
        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Arrange
        Assert.IsType<StatusCodeResult>(res.Result);

        var resObj = res.Result as StatusCodeResult;

        Assert.Equal(StatusCodes.Status500InternalServerError, resObj?.StatusCode);
    }
    
    [Fact]
    public async Task WhenEmptyDb_ReturnsNoContent()
    {
        // Arrange
        var mockSet = new List<Api.Database.Entities.Url>().AsQueryable().BuildMockDbSet();
        _context.Setup(m => m.Urls).Returns(mockSet.Object);
        
        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };
        
        var req = new GetAllUserUrlsRequest
        {
            Page = 1
        };
        
        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Arrange
        Assert.IsType<NoContentResult>(res.Result);
    }
    
    [Fact]
    public async Task WhenPageOutOfIndex_ReturnsNoContent()
    {
        // Arrange
        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };
        
        var req = new GetAllUserUrlsRequest
        {
            Page = 144
        };
        
        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Arrange
        Assert.IsType<NotFoundResult>(res.Result);
    }

    [Fact]
    public async Task WhenValidPage_ButUserHasNoUrls_ReturnsNotFound()
    {
        // Arrange
        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };
        
        var req = new GetAllUserUrlsRequest
        {
            Page = 156
        };
        
        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Arrange
        Assert.IsType<NotFoundResult>(res.Result);
    }
    
    [Fact]
    public async Task ReturnsOk()
    {
        // Arrange
        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };
        
        var req = new GetAllUserUrlsRequest
        {
            Page = 2
        };
        
        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Arrange
        var resVal = Assert.IsType<OkObjectResult>(res.Result);
        var resObj = Assert.IsType<GetAllUserUrlsResponse>(resVal.Value);
        
        var pageResults = 5f;

        var expected = _context.Object.Urls
            .Where(x => x.UrlDetails != null && x.UrlDetails.UserId == UrlHelpers.USER1_UUID)
            .Skip((req.Page - 1) * (int) pageResults)
            .Take((int) pageResults);
        
        expected.Should().BeEquivalentTo(resObj.Urls, 
            o => o.Excluding(si => si.ShortUrl));

        var urlsCount = _context.Object.Urls
            .Where(x => x.UrlDetails != null && x.UrlDetails.UserId == UrlHelpers.USER1_UUID)
            .LongCount();
        var pageCount = Math.Ceiling(urlsCount / pageResults);
        
        Assert.Equal(pageCount, resObj.Pages);
        Assert.Equal(req.Page, resObj.CurrentPage);
        resObj.Urls.Should().BeInAscendingOrder(x => x.Id);
    }
}