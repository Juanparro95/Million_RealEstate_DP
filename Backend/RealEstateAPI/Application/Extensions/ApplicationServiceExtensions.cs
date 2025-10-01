using Microsoft.Extensions.DependencyInjection;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Application.Services;
using System.Reflection;

namespace RealEstateAPI.Application.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Registrar AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Registrar servicios de aplicación
        services.AddScoped<IPropertyService, PropertyService>();

        return services;
    }
}