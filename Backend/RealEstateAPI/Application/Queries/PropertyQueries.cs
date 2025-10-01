using MediatR;
using RealEstateAPI.Application.DTOs;

namespace RealEstateAPI.Application.Queries;

// Query para obtener todas las propiedades con filtros
public class GetPropertiesQuery : IRequest<IEnumerable<PropertyListDto>>
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

// Query para obtener una propiedad por ID
public class GetPropertyByIdQuery : IRequest<PropertyDto?>
{
    public string Id { get; set; }
    
    public GetPropertyByIdQuery(string id)
    {
        Id = id;
    }
}

// Query para obtener propiedades de un propietario
public class GetPropertiesByOwnerQuery : IRequest<IEnumerable<PropertyListDto>>
{
    public string IdOwner { get; set; }
    
    public GetPropertiesByOwnerQuery(string idOwner)
    {
        IdOwner = idOwner;
    }
}

// Query para obtener el detalle completo de una propiedad
public class GetPropertyDetailQuery : IRequest<PropertyDto?>
{
    public string Id { get; set; }
    
    public GetPropertyDetailQuery(string id)
    {
        Id = id;
    }
}