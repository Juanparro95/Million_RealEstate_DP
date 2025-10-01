using AutoMapper;
using MediatR;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Application.Queries;
using RealEstateAPI.Domain.Repositories;

namespace RealEstateAPI.Application.Handlers;

public class GetPropertiesQueryHandler : IRequestHandler<GetPropertiesQuery, IEnumerable<PropertyListDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public GetPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyListDto>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
    {
        var properties = await _propertyRepository.GetPropertiesByFilterAsync(
            request.Name,
            request.Address,
            request.MinPrice,
            request.MaxPrice);

        var propertiesWithDetails = new List<PropertyListDto>();

        foreach (var property in properties)
        {
            var propertyWithDetails = await _propertyRepository.GetPropertyCompleteAsync(property.Id);
            if (propertyWithDetails != null)
            {
                var dto = _mapper.Map<PropertyListDto>(propertyWithDetails);
                propertiesWithDetails.Add(dto);
            }
        }

        // Implementar paginación simple
        var skip = (request.Page - 1) * request.PageSize;
        return propertiesWithDetails.Skip(skip).Take(request.PageSize);
    }
}

public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto?>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public GetPropertyByIdQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<PropertyDto?> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.Id);
        return property != null ? _mapper.Map<PropertyDto>(property) : null;
    }
}

public class GetPropertyDetailQueryHandler : IRequestHandler<GetPropertyDetailQuery, PropertyDto?>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public GetPropertyDetailQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<PropertyDto?> Handle(GetPropertyDetailQuery request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetPropertyCompleteAsync(request.Id);
        return property != null ? _mapper.Map<PropertyDto>(property) : null;
    }
}

public class GetPropertiesByOwnerQueryHandler : IRequestHandler<GetPropertiesByOwnerQuery, IEnumerable<PropertyListDto>>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public GetPropertiesByOwnerQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyListDto>> Handle(GetPropertiesByOwnerQuery request, CancellationToken cancellationToken)
    {
        var properties = await _propertyRepository.GetPropertiesByOwnerAsync(request.IdOwner);
        
        var propertiesWithDetails = new List<PropertyListDto>();

        foreach (var property in properties)
        {
            var propertyWithDetails = await _propertyRepository.GetPropertyCompleteAsync(property.Id);
            if (propertyWithDetails != null)
            {
                var dto = _mapper.Map<PropertyListDto>(propertyWithDetails);
                propertiesWithDetails.Add(dto);
            }
        }

        return propertiesWithDetails;
    }
}