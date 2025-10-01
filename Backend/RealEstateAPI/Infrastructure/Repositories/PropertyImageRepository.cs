using MongoDB.Driver;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Domain.Repositories;
using RealEstateAPI.Infrastructure.Data;

namespace RealEstateAPI.Infrastructure.Repositories;

public class PropertyImageRepository : GenericRepository<PropertyImage>, IPropertyImageRepository
{
    public PropertyImageRepository(MongoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PropertyImage>> GetImagesByPropertyAsync(string idProperty)
    {
        var filter = Builders<PropertyImage>.Filter.And(
            Builders<PropertyImage>.Filter.Eq(pi => pi.IdProperty, idProperty),
            Builders<PropertyImage>.Filter.Eq(pi => pi.Enabled, true)
        );
        
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<PropertyImage?> GetMainImageByPropertyAsync(string idProperty)
    {
        var filter = Builders<PropertyImage>.Filter.And(
            Builders<PropertyImage>.Filter.Eq(pi => pi.IdProperty, idProperty),
            Builders<PropertyImage>.Filter.Eq(pi => pi.Enabled, true)
        );
        
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    protected override string GetEntityId(PropertyImage entity) => entity.Id;
}

public class PropertyTraceRepository : GenericRepository<PropertyTrace>, IPropertyTraceRepository
{
    public PropertyTraceRepository(MongoDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PropertyTrace>> GetTracesByPropertyAsync(string idProperty)
    {
        var filter = Builders<PropertyTrace>.Filter.Eq(pt => pt.IdProperty, idProperty);
        var sort = Builders<PropertyTrace>.Sort.Descending(pt => pt.DateSale);
        
        return await _collection.Find(filter).Sort(sort).ToListAsync();
    }

    public async Task<IEnumerable<PropertyTrace>> GetTracesByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var filter = Builders<PropertyTrace>.Filter.And(
            Builders<PropertyTrace>.Filter.Gte(pt => pt.DateSale, startDate),
            Builders<PropertyTrace>.Filter.Lte(pt => pt.DateSale, endDate)
        );
        
        var sort = Builders<PropertyTrace>.Sort.Descending(pt => pt.DateSale);
        
        return await _collection.Find(filter).Sort(sort).ToListAsync();
    }

    protected override string GetEntityId(PropertyTrace entity) => entity.Id;
}