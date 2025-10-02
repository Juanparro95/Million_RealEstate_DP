using MongoDB.Driver;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace RealEstateAPI.Infrastructure.Data;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public virtual IMongoCollection<T> GetCollection<T>() where T : BaseEntity
    {
        var collectionName = GetCollectionName<T>();
        return _database.GetCollection<T>(collectionName);
    }

    private static string GetCollectionName<T>() where T : BaseEntity
    {
        var attribute = typeof(T).GetCustomAttribute<BsonCollectionAttribute>();
        return attribute?.CollectionName ?? typeof(T).Name.ToLowerInvariant();
    }

    // Collections específicas
    public IMongoCollection<Owner> Owners => GetCollection<Owner>();
    public IMongoCollection<Property> Properties => GetCollection<Property>();
    public IMongoCollection<PropertyImage> PropertyImages => GetCollection<PropertyImage>();
    public IMongoCollection<PropertyTrace> PropertyTraces => GetCollection<PropertyTrace>();
}