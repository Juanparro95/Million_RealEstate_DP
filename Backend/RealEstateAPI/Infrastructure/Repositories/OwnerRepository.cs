using MongoDB.Driver;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Domain.Repositories;
using RealEstateAPI.Infrastructure.Data;

namespace RealEstateAPI.Infrastructure.Repositories;

public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
{
    public OwnerRepository(MongoDbContext context) : base(context)
    {
    }

    public async Task<Owner?> GetByIdOwnerAsync(string idOwner)
    {
        var filter = Builders<Owner>.Filter.Eq(o => o.IdOwner, idOwner);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Owner>> GetOwnersByNameAsync(string name)
    {
        var filter = Builders<Owner>.Filter.Regex(o => o.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
        return await _collection.Find(filter).ToListAsync();
    }

    protected override string GetEntityId(Owner entity) => entity.Id;
}