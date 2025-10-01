using RealEstateAPI.Application.DTOs;

namespace RealEstateAPI.Application.Interfaces;

public interface IPropertyService
{
    Task<IEnumerable<PropertyListDto>> GetPropertiesAsync(
        string? name = null,
        string? address = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int page = 1,
        int pageSize = 10);
        
    Task<PropertyDto?> GetPropertyByIdAsync(string id);
    Task<PropertyDto?> GetPropertyDetailAsync(string id);
    Task<IEnumerable<PropertyListDto>> GetPropertiesByOwnerAsync(string idOwner);
    Task<PropertyDto> CreatePropertyAsync(CreatePropertyRequest request);
    Task<PropertyDto?> UpdatePropertyAsync(string id, UpdatePropertyRequest request);
    Task<bool> DeletePropertyAsync(string id);
}

// Request models para el servicio
public class CreatePropertyRequest
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public string IdOwner { get; set; } = string.Empty;
}

public class UpdatePropertyRequest
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
}