using AutoMapper;
using MediatR;
using MongoDB.Bson;
using RealEstateAPI.Application.Commands;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Domain.Repositories;

namespace RealEstateAPI.Application.Handlers;

public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyDto>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public CreatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<PropertyDto> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = new Property
        {
            IdProperty = ObjectId.GenerateNewId().ToString(),
            Name = request.Name,
            Address = request.Address,
            Price = request.Price,
            CodeInternal = request.CodeInternal,
            Year = request.Year,
            IdOwner = request.IdOwner,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdProperty = await _propertyRepository.AddAsync(property);
        return _mapper.Map<PropertyDto>(createdProperty);
    }
}

public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, PropertyDto?>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<PropertyDto?> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.Id);
        if (property == null)
            return null;

        // Actualizar propiedades
        property.Name = request.Name;
        property.Address = request.Address;
        property.Price = request.Price;
        property.CodeInternal = request.CodeInternal;
        property.Year = request.Year;
        property.UpdatedAt = DateTime.UtcNow;

        await _propertyRepository.UpdateAsync(property);
        return _mapper.Map<PropertyDto>(property);
    }
}

public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, bool>
{
    private readonly IPropertyRepository _propertyRepository;

    public DeletePropertyCommandHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<bool> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.Id);
        if (property == null)
            return false;

        // Soft delete
        property.IsActive = false;
        property.UpdatedAt = DateTime.UtcNow;
        
        await _propertyRepository.UpdateAsync(property);
        return true;
    }
}

public class AddPropertyImageCommandHandler : IRequestHandler<AddPropertyImageCommand, PropertyImageDto>
{
    private readonly IPropertyImageRepository _propertyImageRepository;
    private readonly IMapper _mapper;

    public AddPropertyImageCommandHandler(IPropertyImageRepository propertyImageRepository, IMapper mapper)
    {
        _propertyImageRepository = propertyImageRepository;
        _mapper = mapper;
    }

    public async Task<PropertyImageDto> Handle(AddPropertyImageCommand request, CancellationToken cancellationToken)
    {
        var propertyImage = new PropertyImage
        {
            IdPropertyImage = ObjectId.GenerateNewId().ToString(),
            IdProperty = request.IdProperty,
            File = request.File,
            Enabled = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdImage = await _propertyImageRepository.AddAsync(propertyImage);
        return _mapper.Map<PropertyImageDto>(createdImage);
    }
}