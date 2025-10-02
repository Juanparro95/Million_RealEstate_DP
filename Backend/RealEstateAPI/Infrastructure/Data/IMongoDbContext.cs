using MongoDB.Driver;
using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Infrastructure.Data;

public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>() where T : BaseEntity;
    
    // Collections específicas
    IMongoCollection<Owner> Owners { get; }
    IMongoCollection<Property> Properties { get; }
    IMongoCollection<PropertyImage> PropertyImages { get; }
    IMongoCollection<PropertyTrace> PropertyTraces { get; }
}