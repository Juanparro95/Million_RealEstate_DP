using AutoMapper;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Queries;
using RealEstateAPI.Application.Services;

namespace RealEstateAPI.Tests.Application;

[TestFixture]
public class PropertyServiceTests
{
    private Mock<IMediator> _mockMediator;
    private PropertyService _propertyService;

    [SetUp]
    public void SetUp()
    {
        _mockMediator = new Mock<IMediator>();
        _propertyService = new PropertyService(_mockMediator.Object);
    }

    [Test]
    public async Task GetPropertiesAsync_WithFilters_CallsMediatorWithCorrectQuery()
    {
        // Arrange
        var expectedProperties = new List<PropertyListDto>
        {
            new PropertyListDto
            {
                Id = "64dd57afed26f8790d97e00f",
                IdProperty = "PROP001",
                Name = "Test Property",
                Address = "Test Address",
                Price = 1000000,
                Year = 2020,
                OwnerName = "Test Owner",
                MainImage = "/test-image.jpg"
            }
        };

        _mockMediator.Setup(m => m.Send(It.IsAny<GetPropertiesQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedProperties);

        // Act
        var result = await _propertyService.GetPropertiesAsync("Casa", "Bogotá", 500000, 2000000, 1, 10);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Test Property");

        _mockMediator.Verify(m => m.Send(
            It.Is<GetPropertiesQuery>(q => 
                q.Name == "Casa" && 
                q.Address == "Bogotá" && 
                q.MinPrice == 500000 && 
                q.MaxPrice == 2000000 && 
                q.Page == 1 && 
                q.PageSize == 10), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithValidId_ReturnsProperty()
    {
        // Arrange
        var propertyId = "64dd57afed26f8790d97e00f";
        var expectedProperty = new PropertyDto
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = "Test Property",
            Address = "Test Address",
            Price = 1000000,
            Year = 2020
        };

        _mockMediator.Setup(m => m.Send(It.IsAny<GetPropertyByIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedProperty);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(propertyId);
        result.Name.Should().Be("Test Property");

        _mockMediator.Verify(m => m.Send(
            It.Is<GetPropertyByIdQuery>(q => q.Id == propertyId), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetPropertyDetailAsync_WithValidId_ReturnsPropertyDetail()
    {
        // Arrange
        var propertyId = "64dd57afed26f8790d97e00f";
        var expectedProperty = new PropertyDto
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = "Test Property",
            Address = "Test Address",
            Price = 1000000,
            Year = 2020
        };

        _mockMediator.Setup(m => m.Send(It.IsAny<GetPropertyDetailQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedProperty);

        // Act
        var result = await _propertyService.GetPropertyDetailAsync(propertyId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(propertyId);

        _mockMediator.Verify(m => m.Send(
            It.Is<GetPropertyDetailQuery>(q => q.Id == propertyId), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetPropertiesByOwnerAsync_WithOwnerId_ReturnsOwnerProperties()
    {
        // Arrange
        var ownerId = "OWNER001";
        var expectedProperties = new List<PropertyListDto>
        {
            new PropertyListDto
            {
                Id = "64dd57afed26f8790d97e00f",
                IdProperty = "PROP001",
                Name = "Property 1",
                OwnerName = "Test Owner"
            },
            new PropertyListDto
            {
                Id = "64dd57afed26f8790d97e010",
                IdProperty = "PROP002",
                Name = "Property 2",
                OwnerName = "Test Owner"
            }
        };

        _mockMediator.Setup(m => m.Send(It.IsAny<GetPropertiesByOwnerQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedProperties);

        // Act
        var result = await _propertyService.GetPropertiesByOwnerAsync(ownerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        _mockMediator.Verify(m => m.Send(
            It.Is<GetPropertiesByOwnerQuery>(q => q.IdOwner == ownerId), 
            It.IsAny<CancellationToken>()), Times.Once);
    }
}