using MediatR;
using RealEstateAPI.Application.Commands;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Application.Queries;

namespace RealEstateAPI.Application.Services;

public class PropertyService : IPropertyService
{
    private readonly IMediator _mediator;

    public PropertyService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<PropertyListDto>> GetPropertiesAsync(
        string? name = null, 
        string? address = null, 
        decimal? minPrice = null, 
        decimal? maxPrice = null, 
        int page = 1, 
        int pageSize = 10)
    {
        var query = new GetPropertiesQuery
        {
            Name = name,
            Address = address,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Page = page,
            PageSize = pageSize
        };

        return await _mediator.Send(query);
    }

    public async Task<PropertyDto?> GetPropertyByIdAsync(string id)
    {
        var query = new GetPropertyByIdQuery(id);
        return await _mediator.Send(query);
    }

    public async Task<PropertyDto?> GetPropertyDetailAsync(string id)
    {
        var query = new GetPropertyDetailQuery(id);
        return await _mediator.Send(query);
    }

    public async Task<IEnumerable<PropertyListDto>> GetPropertiesByOwnerAsync(string idOwner)
    {
        var query = new GetPropertiesByOwnerQuery(idOwner);
        return await _mediator.Send(query);
    }

    public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyRequest request)
    {
        var command = new CreatePropertyCommand
        {
            Name = request.Name,
            Address = request.Address,
            Price = request.Price,
            CodeInternal = request.CodeInternal,
            Year = request.Year,
            IdOwner = request.IdOwner
        };

        return await _mediator.Send(command);
    }

    public async Task<PropertyDto?> UpdatePropertyAsync(string id, UpdatePropertyRequest request)
    {
        var command = new UpdatePropertyCommand
        {
            Id = id,
            Name = request.Name,
            Address = request.Address,
            Price = request.Price,
            CodeInternal = request.CodeInternal,
            Year = request.Year
        };

        return await _mediator.Send(command);
    }

    public async Task<bool> DeletePropertyAsync(string id)
    {
        var command = new DeletePropertyCommand(id);
        return await _mediator.Send(command);
    }
}