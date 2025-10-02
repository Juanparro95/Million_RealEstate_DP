using MongoDB.Driver;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Domain.Repositories;
using RealEstateAPI.Infrastructure.Data;

namespace RealEstateAPI.Infrastructure.Repositories;

public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
{
    public PropertyRepository(IMongoDbContext context) : base(context)
    {
    }

    public async Task<Property?> GetByIdPropertyAsync(string idProperty)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.IdProperty, idProperty);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(string idOwner)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.IdOwner, idOwner);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetPropertiesByFilterAsync(
        string? name = null, 
        string? address = null, 
        decimal? minPrice = null, 
        decimal? maxPrice = null)
    {
        var filterBuilder = Builders<Property>.Filter;
        var filters = new List<FilterDefinition<Property>>();

        // Log de entrada
        Console.WriteLine($"[REPO] GetPropertiesByFilterAsync - name: {name}, address: {address}, minPrice: {minPrice}, maxPrice: {maxPrice}");

        // Filtro por nombre (búsqueda parcial, case-insensitive)
        if (!string.IsNullOrEmpty(name))
        {
            filters.Add(filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i")));
        }

        // Filtro por dirección (búsqueda parcial, case-insensitive)
        if (!string.IsNullOrEmpty(address))
        {
            filters.Add(filterBuilder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(address, "i")));
        }

        // Filtro por rango de precios
        if (minPrice.HasValue)
        {
            filters.Add(filterBuilder.Gte(p => p.Price, minPrice.Value));
        }

        if (maxPrice.HasValue)
        {
            filters.Add(filterBuilder.Lte(p => p.Price, maxPrice.Value));
        }

        // Solo propiedades activas
        filters.Add(filterBuilder.Eq(p => p.IsActive, true));

        var combinedFilter = filters.Count > 1 ? filterBuilder.And(filters) : filters[0];
        
        Console.WriteLine($"[REPO] Filter count: {filters.Count}, Combined filter: {combinedFilter}");
        
        var result = await _collection.Find(combinedFilter).ToListAsync();
        
        Console.WriteLine($"[REPO] Found {result.Count} properties");
        
        return result;
    }

    public async Task<Property?> GetPropertyWithImagesAsync(string id)
    {
        var property = await GetByIdAsync(id);
        if (property != null)
        {
            var images = await _context.PropertyImages
                .Find(pi => pi.IdProperty == property.IdProperty)
                .ToListAsync();
            
            property.PropertyImages = images;
        }
        
        return property;
    }

    public async Task<Property?> GetPropertyWithTracesAsync(string id)
    {
        var property = await GetByIdAsync(id);
        if (property != null)
        {
            var traces = await _context.PropertyTraces
                .Find(pt => pt.IdProperty == property.IdProperty)
                .ToListAsync();
            
            property.PropertyTraces = traces;
        }
        
        return property;
    }

    public async Task<Property?> GetPropertyCompleteAsync(string id)
    {
        // Primero buscar por _id de MongoDB, luego por IdProperty si no se encuentra
        var property = await GetByIdAsync(id);
        if (property == null)
        {
            property = await GetByIdPropertyAsync(id);
        }
        
        if (property != null)
        {
            // Cargar imágenes
            var images = await _context.PropertyImages
                .Find(pi => pi.IdProperty == property.IdProperty)
                .ToListAsync();
            
            // Cargar traces
            var traces = await _context.PropertyTraces
                .Find(pt => pt.IdProperty == property.IdProperty)
                .ToListAsync();
            
            // Cargar owner
            var owner = await _context.Owners
                .Find(o => o.IdOwner == property.IdOwner)
                .FirstOrDefaultAsync();

            property.PropertyImages = images;
            property.PropertyTraces = traces;
            property.Owner = owner!;
        }
        
        return property;
    }

    protected override string GetEntityId(Property entity) => entity.Id;
}