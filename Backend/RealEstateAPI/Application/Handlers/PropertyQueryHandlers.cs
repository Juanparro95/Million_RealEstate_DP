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
    private readonly ILogger<GetPropertiesQueryHandler> _logger;

    public GetPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper, ILogger<GetPropertiesQueryHandler> logger)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PropertyListDto>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting GetPropertiesQuery with filters: Name={Name}, Address={Address}, MinPrice={MinPrice}, MaxPrice={MaxPrice}",
            request.Name, request.Address, request.MinPrice, request.MaxPrice);

        var properties = await _propertyRepository.GetPropertiesByFilterAsync(
            request.Name,
            request.Address,
            request.MinPrice,
            request.MaxPrice);

        _logger.LogInformation("Found {Count} properties from repository", properties.Count());

        var propertiesWithDetails = new List<PropertyListDto>();

        foreach (var property in properties)
        {
            _logger.LogInformation("Processing property {PropertyId} - {PropertyName}", property.Id, property.Name);
            
            try
            {
                // Cargar la propiedad completa con imágenes y relaciones
                var propertyWithDetails = await _propertyRepository.GetPropertyCompleteAsync(property.IdProperty);
                if (propertyWithDetails != null)
                {
                    var dto = _mapper.Map<PropertyListDto>(propertyWithDetails);
                    propertiesWithDetails.Add(dto);
                    _logger.LogInformation("Successfully mapped property {PropertyId} to DTO with images", property.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping property {PropertyId} to DTO", property.Id);
            }
        }

        _logger.LogInformation("Total properties with details: {Count}", propertiesWithDetails.Count);

        // Paginación simple
        var skip = (request.Page - 1) * request.PageSize;
        var result = propertiesWithDetails.Skip(skip).Take(request.PageSize);
        
        _logger.LogInformation("After pagination (skip={Skip}, take={Take}): {Count} properties", skip, request.PageSize, result.Count());

        return result;
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
        var property = await _propertyRepository.GetPropertyCompleteAsync(request.Id);
        return property != null ? _mapper.Map<PropertyDto>(property) : null;
    }
}

public class GetPropertyDetailQueryHandler : IRequestHandler<GetPropertyDetailQuery, PropertyDto?>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPropertyDetailQueryHandler> _logger;

    public GetPropertyDetailQueryHandler(IPropertyRepository propertyRepository, IMapper mapper, ILogger<GetPropertyDetailQueryHandler> logger)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PropertyDto?> Handle(GetPropertyDetailQuery request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetPropertyCompleteAsync(request.Id);
        _logger.LogInformation("Getting property with ID: {Id}", property);
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