using AutoMapper;
using RealEstateAPI.Application.DTOs;
using RealEstateAPI.Domain.Entities;

namespace RealEstateAPI.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Owner mappings
        CreateMap<Owner, OwnerDto>().ReverseMap();
        
        // Property mappings
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => 
                src.PropertyImages != null && src.PropertyImages.Any(pi => pi.Enabled)
                    ? src.PropertyImages.First(pi => pi.Enabled).File
                    : string.Empty))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PropertyImages))
            .ForMember(dest => dest.Traces, opt => opt.MapFrom(src => src.PropertyTraces))
            .ReverseMap();
            
        CreateMap<Property, PropertyListDto>()
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => "Propietario"))
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => 
                src.PropertyImages != null && src.PropertyImages.Any(pi => pi.Enabled)
                    ? src.PropertyImages.First(pi => pi.Enabled).File
                    : "/placeholder-property.jpg"));
        
        // PropertyImage mappings
        CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();
        
        // PropertyTrace mappings
        CreateMap<PropertyTrace, PropertyTraceDto>().ReverseMap();
    }
}