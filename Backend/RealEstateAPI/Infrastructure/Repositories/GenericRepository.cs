using MongoDB.Driver;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Domain.Repositories;
using RealEstateAPI.Infrastructure.Data;
using System.Linq.Expressions;

namespace RealEstateAPI.Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly MongoDbContext _context;
    protected readonly IMongoCollection<T> _collection;

    protected GenericRepository(MongoDbContext context)
    {
        _context = context;
        _collection = _context.GetCollection<T>();
    }

    public virtual async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        var filter = Builders<T>.Filter.Eq("_id", GetEntityId(entity));
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public virtual async Task DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
    }

    public virtual async Task<bool> ExistsAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).AnyAsync();
    }

    public virtual async Task<int> CountAsync()
    {
        return (int)await _collection.CountDocumentsAsync(_ => true);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return (int)await _collection.CountDocumentsAsync(predicate);
    }

    protected abstract string GetEntityId(T entity);
}