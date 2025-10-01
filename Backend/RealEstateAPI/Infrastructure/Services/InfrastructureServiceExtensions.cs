using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RealEstateAPI.Infrastructure.Configuration;
using RealEstateAPI.Infrastructure.Data;
using RealEstateAPI.Infrastructure.Repositories;
using RealEstateAPI.Domain.Repositories;

namespace RealEstateAPI.Infrastructure.Services;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Configuración de MongoDB
        services.Configure<MongoDbSettings>(
            configuration.GetSection("MongoDbSettings"));

        // Registrar DbContext
        services.AddSingleton<MongoDbContext>();

        // Registrar repositorios
        services.AddScoped<IOwnerRepository, OwnerRepository>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
        services.AddScoped<IPropertyTraceRepository, PropertyTraceRepository>();

        return services;
    }
}