using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Domain.Repositories;

public interface IOwnerRepository : IGenericRepository<Owner>
{
    Task<Owner?> GetByIdOwnerAsync(string idOwner);
    Task<IEnumerable<Owner>> GetOwnersByNameAsync(string name);
}

public interface IPropertyRepository : IGenericRepository<Property>
{
    Task<Property?> GetByIdPropertyAsync(string idProperty);
    Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(string idOwner);
    Task<IEnumerable<Property>> GetPropertiesByFilterAsync(
        string? name = null,
        string? address = null,
        decimal? minPrice = null,
        decimal? maxPrice = null);
    Task<Property?> GetPropertyWithImagesAsync(string id);
    Task<Property?> GetPropertyWithTracesAsync(string id);
    Task<Property?> GetPropertyCompleteAsync(string id);
}

public interface IPropertyImageRepository : IGenericRepository<PropertyImage>
{
    Task<IEnumerable<PropertyImage>> GetImagesByPropertyAsync(string idProperty);
    Task<PropertyImage?> GetMainImageByPropertyAsync(string idProperty);
}

public interface IPropertyTraceRepository : IGenericRepository<PropertyTrace>
{
    Task<IEnumerable<PropertyTrace>> GetTracesByPropertyAsync(string idProperty);
    Task<IEnumerable<PropertyTrace>> GetTracesByDateRangeAsync(DateTime startDate, DateTime endDate);
}