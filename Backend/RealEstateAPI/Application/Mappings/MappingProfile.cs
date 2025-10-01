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
                src.PropertyImages.FirstOrDefault(pi => pi.Enabled)!.File ?? string.Empty))
            .ReverseMap();
            
        CreateMap<Property, PropertyListDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => 
                src.PropertyImages.FirstOrDefault(pi => pi.Enabled)!.File ?? string.Empty));
        
        // PropertyImage mappings
        CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();
        
        // PropertyTrace mappings
        CreateMap<PropertyTrace, PropertyTraceDto>().ReverseMap();
    }
}