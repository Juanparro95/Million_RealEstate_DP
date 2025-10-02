using MongoDB.Bson;
using MongoDB.Driver;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Domain.Repositories;
using RealEstateAPI.Infrastructure.Data;
using System.Linq.Expressions;

namespace RealEstateAPI.Infrastructure.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly IMongoDbContext _context;
    protected readonly IMongoCollection<T> _collection;

    protected GenericRepository(IMongoDbContext context)
    {
        _context = context;
        _collection = _context.GetCollection<T>();
    }

    public virtual async Task<T?> GetByIdAsync(string id)
    {
        // Convertir el string a ObjectId para la búsqueda
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return null; // ID inválido
        }
        
        var filter = Builders<T>.Filter.Eq("_id", objectId);
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
        if (!ObjectId.TryParse(GetEntityId(entity), out ObjectId objectId))
        {
            throw new ArgumentException("Invalid entity ID format");
        }
        
        var filter = Builders<T>.Filter.Eq("_id", objectId);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public virtual async Task DeleteAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return; // ID inválido, no hacer nada
        }
        
        var filter = Builders<T>.Filter.Eq("_id", objectId);
        await _collection.DeleteOneAsync(filter);
    }

    public virtual async Task<bool> ExistsAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return false; // ID inválido
        }
        
        var filter = Builders<T>.Filter.Eq("_id", objectId);
        var count = await _collection.CountDocumentsAsync(filter);
        return count > 0;
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