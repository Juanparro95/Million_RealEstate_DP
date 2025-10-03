using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Handlers;
using RealEstateAPI.Application.Queries;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Domain.Repositories;

namespace RealEstateAPI.Tests.Application;

[TestFixture]
public class PropertyQueryHandlersTests
{
    private Mock<IPropertyRepository> _mockRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<ILogger<GetPropertiesQueryHandler>> _mockLogger;
    private Mock<ILogger<GetPropertyDetailQueryHandler>> _mockDetailLogger;
    private GetPropertiesQueryHandler _getPropertiesHandler;
    private GetPropertyByIdQueryHandler _getPropertyByIdHandler;
    private GetPropertyDetailQueryHandler _getPropertyDetailHandler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<IPropertyRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GetPropertiesQueryHandler>>();
        _mockDetailLogger = new Mock<ILogger<GetPropertyDetailQueryHandler>>();
        
        _getPropertiesHandler = new GetPropertiesQueryHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
        _getPropertyByIdHandler = new GetPropertyByIdQueryHandler(_mockRepository.Object, _mockMapper.Object);
        _getPropertyDetailHandler = new GetPropertyDetailQueryHandler(_mockRepository.Object, _mockMapper.Object, _mockDetailLogger.Object);
    }

    [Test]
    public async Task GetPropertiesQueryHandler_Handle_ReturnsFilteredProperties()
    {
        // Arrange
        var query = new GetPropertiesQuery
        {
            Name = "Casa",
            Address = "Bogotá",
            MinPrice = 500000,
            MaxPrice = 2000000,
            Page = 1,
            PageSize = 10
        };

        var properties = new List<Property>
        {
            new Property
            {
                Id = "64dd57afed26f8790d97e00f",
                IdProperty = "PROP001",
                Name = "Casa Familiar",
                Address = "Carrera 13 #85-40, Zona Rosa, Bogotá",
                Price = 850000000,
                Year = 2020
            }
        };

        var expectedDtos = new List<PropertyListDto>
        {
            new PropertyListDto
            {
                Id = "64dd57afed26f8790d97e00f",
                IdProperty = "PROP001",
                Name = "Casa Familiar",
                Address = "Carrera 13 #85-40, Zona Rosa, Bogotá",
                Price = 850000000,
                Year = 2020,
                OwnerName = "Test Owner",
                MainImage = "/placeholder-property.jpg"
            }
        };

        _mockRepository.Setup(r => r.GetPropertiesByFilterAsync(
            query.Name, query.Address, query.MinPrice, query.MaxPrice))
            .ReturnsAsync(properties);

        _mockRepository.Setup(r => r.GetPropertyCompleteAsync(It.IsAny<string>()))
            .ReturnsAsync(properties[0]);

        _mockMapper.Setup(m => m.Map<PropertyListDto>(It.IsAny<Property>()))
                  .Returns(expectedDtos[0]);

        // Act
        var result = await _getPropertiesHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Casa Familiar");

        _mockRepository.Verify(r => r.GetPropertiesByFilterAsync(
            query.Name, query.Address, query.MinPrice, query.MaxPrice), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdQueryHandler_Handle_WithValidId_ReturnsProperty()
    {
        // Arrange
        var propertyId = "64dd57afed26f8790d97e00f";
        var query = new GetPropertyByIdQuery(propertyId);

        var property = new Property
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = "Test Property",
            Address = "Test Address",
            Price = 1000000,
            Year = 2020
        };

        var expectedDto = new PropertyDto
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = "Test Property",
            Address = "Test Address",
            Price = 1000000,
            Year = 2020
        };

        _mockRepository.Setup(r => r.GetPropertyCompleteAsync(propertyId))
                      .ReturnsAsync(property);

        _mockMapper.Setup(m => m.Map<PropertyDto>(property))
                  .Returns(expectedDto);

        // Act
        var result = await _getPropertyByIdHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(propertyId);
        result.Name.Should().Be("Test Property");

        _mockRepository.Verify(r => r.GetPropertyCompleteAsync(propertyId), Times.Once);
        _mockMapper.Verify(m => m.Map<PropertyDto>(property), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdQueryHandler_Handle_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        var propertyId = "nonexistent";
        var query = new GetPropertyByIdQuery(propertyId);

        _mockRepository.Setup(r => r.GetPropertyCompleteAsync(propertyId))
                      .ReturnsAsync((Property?)null);

        // Act
        var result = await _getPropertyByIdHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        _mockRepository.Verify(r => r.GetPropertyCompleteAsync(propertyId), Times.Once);
        _mockMapper.Verify(m => m.Map<PropertyDto>(It.IsAny<Property>()), Times.Never);
    }

    [Test]
    public async Task GetPropertyDetailQueryHandler_Handle_WithValidId_ReturnsPropertyWithDetails()
    {
        // Arrange
        var propertyId = "64dd57afed26f8790d97e00f";
        var query = new GetPropertyDetailQuery(propertyId);

        var property = new Property
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = "Test Property",
            Address = "Test Address",
            Price = 1000000,
            Year = 2020,
            IdOwner = "OWNER001"
        };

        var expectedDto = new PropertyDto
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = "Test Property",
            Address = "Test Address",
            Price = 1000000,
            Year = 2020
        };

        _mockRepository.Setup(r => r.GetPropertyCompleteAsync(propertyId))
                      .ReturnsAsync(property);

        _mockMapper.Setup(m => m.Map<PropertyDto>(It.IsAny<Property>()))
                  .Returns(expectedDto);

        // Act
        var result = await _getPropertyDetailHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(propertyId);

        _mockRepository.Verify(r => r.GetPropertyCompleteAsync(propertyId), Times.Once);
    }

    [Test]
    public async Task GetPropertiesQueryHandler_Handle_WithPagination_ReturnsCorrectPage()
    {
        // Arrange
        var query = new GetPropertiesQuery
        {
            Page = 2,
            PageSize = 2
        };

        var properties = new List<Property>
        {
            new Property { Id = "1", IdProperty = "PROP1", Name = "Property 1" },
            new Property { Id = "2", IdProperty = "PROP2", Name = "Property 2" },
            new Property { Id = "3", IdProperty = "PROP3", Name = "Property 3" },
            new Property { Id = "4", IdProperty = "PROP4", Name = "Property 4" }
        };

        var dtos = properties.Select(p => new PropertyListDto 
        { 
            Id = p.Id, 
            Name = p.Name,
            IdProperty = $"PROP{p.Id}",
            Address = "Test Address",
            Price = 1000000,
            Year = 2020,
            OwnerName = "Test Owner",
            MainImage = "/test.jpg"
        }).ToList();

        _mockRepository.Setup(r => r.GetPropertiesByFilterAsync(null, null, null, null))
                      .ReturnsAsync(properties);

        _mockRepository.Setup(r => r.GetPropertyCompleteAsync(It.IsAny<string>()))
                      .ReturnsAsync((string id) => 
                      {
                          var prop = properties.FirstOrDefault(p => p.IdProperty == id);
                          return prop ?? properties[0];
                      });

        _mockMapper.Setup(m => m.Map<PropertyListDto>(It.IsAny<Property>()))
                  .Returns<Property>(p => dtos.First(d => d.Id == p.Id));

        // Act
        var result = await _getPropertiesHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().Id.Should().Be("3");
        result.Last().Id.Should().Be("4");
    }
}