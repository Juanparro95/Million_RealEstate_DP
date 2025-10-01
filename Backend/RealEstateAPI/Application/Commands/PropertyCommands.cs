using MediatR;
using RealEstateAPI.Application.DTOs;

namespace RealEstateAPI.Application.Commands;

// Command para crear una nueva propiedad
public class CreatePropertyCommand : IRequest<PropertyDto>
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public string IdOwner { get; set; } = string.Empty;
}

// Command para actualizar una propiedad
public class UpdatePropertyCommand : IRequest<PropertyDto?>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
}

// Command para eliminar una propiedad (soft delete)
public class DeletePropertyCommand : IRequest<bool>
{
    public string Id { get; set; }
    
    public DeletePropertyCommand(string id)
    {
        Id = id;
    }
}

// Command para agregar una imagen a una propiedad
public class AddPropertyImageCommand : IRequest<PropertyImageDto>
{
    public string IdProperty { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
}