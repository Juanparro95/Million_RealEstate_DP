using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Presentation.Controllers;

namespace RealEstateAPI.Tests.Presentation;

[TestFixture]
public class PropertiesControllerTests
{
    private Mock<IPropertyService> _mockPropertyService;
    private Mock<ILogger<PropertiesController>> _mockLogger;
    private PropertiesController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockPropertyService = new Mock<IPropertyService>();
        _mockLogger = new Mock<ILogger<PropertiesController>>();
        _controller = new PropertiesController(_mockPropertyService.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetProperties_WithoutFilters_ReturnsOkWithProperties()
    {
        // Arrange
        var expectedProperties = new List<PropertyListDto>
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

        _mockPropertyService.Setup(s => s.GetPropertiesAsync(null, null, null, null, 1, 10))
                           .ReturnsAsync(expectedProperties);

        // Act
        var result = await _controller.GetProperties();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<PropertyListDto>>>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        
        var properties = okResult.Value as IEnumerable<PropertyListDto>;
        properties.Should().NotBeNull();
        properties.Should().HaveCount(1);
        properties!.First().Name.Should().Be("Casa Familiar");
    }

    [Test]
    public async Task GetProperties_WithFilters_ReturnsFilteredProperties()
    {
        // Arrange
        var name = "Casa";
        var address = "Bogotá";
        var minPrice = 500000m;
        var maxPrice = 2000000m;
        var page = 1;
        var pageSize = 5;

        var expectedProperties = new List<PropertyListDto>
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

        _mockPropertyService.Setup(s => s.GetPropertiesAsync(name, address, minPrice, maxPrice, page, pageSize))
                           .ReturnsAsync(expectedProperties);

        // Act
        var result = await _controller.GetProperties(name, address, minPrice, maxPrice, page, pageSize);

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<PropertyListDto>>>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        _mockPropertyService.Verify(s => s.GetPropertiesAsync(name, address, minPrice, maxPrice, page, pageSize), Times.Once);
    }

    [Test]
    public async Task GetProperty_WithValidId_ReturnsOkWithProperty()
    {
        // Arrange
        var propertyId = "64dd57afed26f8790d97e00f";
        var expectedProperty = new PropertyDto
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = "Casa Familiar",
            Address = "Carrera 13 #85-40, Zona Rosa, Bogotá",
            Price = 850000000,
            Year = 2020,
            CodeInternal = "INTERNAL001"
        };

        _mockPropertyService.Setup(s => s.GetPropertyDetailAsync(propertyId))
                           .ReturnsAsync(expectedProperty);

        // Act
        var result = await _controller.GetProperty(propertyId);

        // Assert
        result.Should().BeOfType<ActionResult<PropertyDto>>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var property = okResult.Value as PropertyDto;
        property.Should().NotBeNull();
        property!.Id.Should().Be(propertyId);
        property.Name.Should().Be("Casa Familiar");
    }

    [Test]
    public async Task GetProperty_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var propertyId = "nonexistent";

        _mockPropertyService.Setup(s => s.GetPropertyDetailAsync(propertyId))
                           .ReturnsAsync((PropertyDto?)null);

        // Act
        var result = await _controller.GetProperty(propertyId);

        // Assert
        result.Should().BeOfType<ActionResult<PropertyDto>>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().Be($"Property with ID {propertyId} not found");
    }

    [Test]
    public async Task GetPropertiesByOwner_WithValidOwnerId_ReturnsOkWithProperties()
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

        _mockPropertyService.Setup(s => s.GetPropertiesByOwnerAsync(ownerId))
                           .ReturnsAsync(expectedProperties);

        // Act
        var result = await _controller.GetPropertiesByOwner(ownerId);

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<PropertyListDto>>>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var properties = okResult.Value as IEnumerable<PropertyListDto>;
        properties.Should().NotBeNull();
        properties.Should().HaveCount(2);
    }

    [Test]
    public async Task CreateProperty_WithValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var request = new CreatePropertyRequest
        {
            Name = "Nueva Casa",
            Address = "Calle 123 #45-67",
            Price = 1500000000,
            CodeInternal = "INTERNAL002",
            Year = 2023,
            IdOwner = "OWNER001"
        };

        var createdProperty = new PropertyDto
        {
            Id = "64dd57afed26f8790d97e015",
            IdProperty = "PROP007",
            Name = request.Name,
            Address = request.Address,
            Price = request.Price,
            CodeInternal = request.CodeInternal,
            Year = request.Year
        };

        _mockPropertyService.Setup(s => s.CreatePropertyAsync(request))
                           .ReturnsAsync(createdProperty);

        // Act
        var result = await _controller.CreateProperty(request);

        // Assert
        result.Should().BeOfType<ActionResult<PropertyDto>>();
        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(201);
        createdResult.ActionName.Should().Be(nameof(PropertiesController.GetProperty));
        createdResult.RouteValues!["id"].Should().Be(createdProperty.Id);

        var property = createdResult.Value as PropertyDto;
        property.Should().NotBeNull();
        property!.Name.Should().Be(request.Name);
    }

    [Test]
    public async Task UpdateProperty_WithValidRequest_ReturnsOkWithUpdatedProperty()
    {
        // Arrange
        var propertyId = "64dd57afed26f8790d97e00f";
        var request = new UpdatePropertyRequest
        {
            Name = "Casa Actualizada",
            Address = "Nueva Dirección 123",
            Price = 2000000000,
            CodeInternal = "UPDATED001",
            Year = 2024
        };

        var updatedProperty = new PropertyDto
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = request.Name,
            Address = request.Address,
            Price = request.Price,
            CodeInternal = request.CodeInternal,
            Year = request.Year
        };

        _mockPropertyService.Setup(s => s.UpdatePropertyAsync(propertyId, request))
                           .ReturnsAsync(updatedProperty);

        // Act
        var result = await _controller.UpdateProperty(propertyId, request);

        // Assert
        result.Should().BeOfType<ActionResult<PropertyDto>>();
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var property = okResult.Value as PropertyDto;
        property.Should().NotBeNull();
        property!.Name.Should().Be(request.Name);
    }

    [Test]
    public async Task UpdateProperty_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var propertyId = "nonexistent";
        var request = new UpdatePropertyRequest
        {
            Name = "Casa Actualizada",
            Address = "Nueva Dirección 123",
            Price = 2000000000,
            CodeInternal = "UPDATED001",
            Year = 2024
        };

        _mockPropertyService.Setup(s => s.UpdatePropertyAsync(propertyId, request))
                           .ReturnsAsync((PropertyDto?)null);

        // Act
        var result = await _controller.UpdateProperty(propertyId, request);

        // Assert
        result.Should().BeOfType<ActionResult<PropertyDto>>();
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Test]
    public async Task DeleteProperty_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var propertyId = "64dd57afed26f8790d97e00f";

        _mockPropertyService.Setup(s => s.DeletePropertyAsync(propertyId))
                           .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteProperty(propertyId);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }

    [Test]
    public async Task DeleteProperty_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        var propertyId = "nonexistent";

        _mockPropertyService.Setup(s => s.DeletePropertyAsync(propertyId))
                           .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteProperty(propertyId);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Test]
    public async Task GetProperties_ServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        _mockPropertyService.Setup(s => s.GetPropertiesAsync(null, null, null, null, 1, 10))
                           .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await _controller.GetProperties();

        // Assert
        result.Should().BeOfType<ActionResult<IEnumerable<PropertyListDto>>>();
        var statusCodeResult = result.Result as ObjectResult;
        statusCodeResult.Should().NotBeNull();
        statusCodeResult!.StatusCode.Should().Be(500);
    }
}