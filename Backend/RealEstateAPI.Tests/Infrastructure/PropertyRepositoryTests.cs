using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Infrastructure.Data;
using RealEstateAPI.Infrastructure.Repositories;

namespace RealEstateAPI.Tests.Infrastructure;

[TestFixture]
public class PropertyRepositoryTests
{
    private Mock<IMongoDbContext> _mockContext;
    private Mock<IMongoCollection<Property>> _mockCollection;
    private PropertyRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _mockContext = new Mock<IMongoDbContext>();
        _mockCollection = new Mock<IMongoCollection<Property>>();
        
        _mockContext.Setup(c => c.GetCollection<Property>()).Returns(_mockCollection.Object);
        _repository = new PropertyRepository(_mockContext.Object);
    }

    [Test]
    public async Task GetByIdAsync_WithValidObjectId_ReturnsProperty()
    {
        // Arrange
        var objectId = ObjectId.GenerateNewId();
        var propertyId = objectId.ToString();
        var expectedProperty = new Property
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = "Test Property",
            Address = "Test Address",
            Price = 1000000,
            Year = 2020,
            IdOwner = "OWNER001"
        };

        var mockCursor = new Mock<IAsyncCursor<Property>>();
        mockCursor.Setup(c => c.Current).Returns(new List<Property> { expectedProperty });
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Property>>(),
            It.IsAny<FindOptions<Property, Property>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _repository.GetByIdAsync(propertyId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(propertyId);
        result.Name.Should().Be("Test Property");
    }

    [Test]
    public async Task GetByIdAsync_WithInvalidObjectId_ReturnsNull()
    {
        // Arrange
        var invalidId = "invalid-id";

        // Act
        var result = await _repository.GetByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetByIdPropertyAsync_WithValidIdProperty_ReturnsProperty()
    {
        // Arrange
        var idProperty = "PROP001";
        var expectedProperty = new Property
        {
            Id = ObjectId.GenerateNewId().ToString(),
            IdProperty = idProperty,
            Name = "Test Property",
            Address = "Test Address",
            Price = 1000000,
            Year = 2020,
            IdOwner = "OWNER001"
        };

        var mockCursor = new Mock<IAsyncCursor<Property>>();
        mockCursor.Setup(c => c.Current).Returns(new List<Property> { expectedProperty });
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Property>>(),
            It.IsAny<FindOptions<Property, Property>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _repository.GetByIdPropertyAsync(idProperty);

        // Assert
        result.Should().NotBeNull();
        result!.IdProperty.Should().Be(idProperty);
        result.Name.Should().Be("Test Property");
    }

    [Test]
    public async Task GetPropertiesByOwnerAsync_WithValidOwnerId_ReturnsProperties()
    {
        // Arrange
        var ownerId = "OWNER001";
        var expectedProperties = new List<Property>
        {
            new Property
            {
                Id = ObjectId.GenerateNewId().ToString(),
                IdProperty = "PROP001",
                Name = "Property 1",
                IdOwner = ownerId
            },
            new Property
            {
                Id = ObjectId.GenerateNewId().ToString(),
                IdProperty = "PROP002",
                Name = "Property 2",
                IdOwner = ownerId
            }
        };

        var mockCursor = new Mock<IAsyncCursor<Property>>();
        mockCursor.Setup(c => c.Current).Returns(expectedProperties);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Property>>(),
            It.IsAny<FindOptions<Property, Property>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _repository.GetPropertiesByOwnerAsync(ownerId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p.IdOwner == ownerId);
    }

    [Test]
    public async Task GetPropertiesByFilterAsync_WithNameFilter_ReturnsFilteredProperties()
    {
        // Arrange
        var nameFilter = "Casa";
        var expectedProperties = new List<Property>
        {
            new Property
            {
                Id = ObjectId.GenerateNewId().ToString(),
                IdProperty = "PROP001",
                Name = "Casa Familiar",
                IsActive = true
            }
        };

        var mockCursor = new Mock<IAsyncCursor<Property>>();
        mockCursor.Setup(c => c.Current).Returns(expectedProperties);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Property>>(),
            It.IsAny<FindOptions<Property, Property>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _repository.GetPropertiesByFilterAsync(name: nameFilter);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Name.Should().Contain("Casa");
    }

    [Test]
    public async Task GetPropertiesByFilterAsync_WithPriceRange_ReturnsFilteredProperties()
    {
        // Arrange
        var minPrice = 500000m;
        var maxPrice = 1500000m;
        var expectedProperties = new List<Property>
        {
            new Property
            {
                Id = ObjectId.GenerateNewId().ToString(),
                IdProperty = "PROP001",
                Name = "Property in Range",
                Price = 1000000m,
                IsActive = true
            }
        };

        var mockCursor = new Mock<IAsyncCursor<Property>>();
        mockCursor.Setup(c => c.Current).Returns(expectedProperties);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
            It.IsAny<FilterDefinition<Property>>(),
            It.IsAny<FindOptions<Property, Property>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        // Act
        var result = await _repository.GetPropertiesByFilterAsync(minPrice: minPrice, maxPrice: maxPrice);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Price.Should().BeInRange(minPrice, maxPrice);
    }

    [Test]
    public async Task ExistsAsync_WithValidObjectId_ReturnsTrue()
    {
        // Arrange
        var objectId = ObjectId.GenerateNewId();
        var propertyId = objectId.ToString();

        // Since AnyAsync is an extension method, we can't mock it directly
        // We'll mock CountDocumentsAsync instead which is what should be used for existence checks
        _mockCollection.Setup(c => c.CountDocumentsAsync(
            It.IsAny<FilterDefinition<Property>>(), 
            It.IsAny<CountOptions>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _repository.ExistsAsync(propertyId);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_WithInvalidObjectId_ReturnsFalse()
    {
        // Arrange
        var invalidId = "invalid-id";

        // Act
        var result = await _repository.ExistsAsync(invalidId);

        // Assert
        result.Should().BeFalse();
    }
}