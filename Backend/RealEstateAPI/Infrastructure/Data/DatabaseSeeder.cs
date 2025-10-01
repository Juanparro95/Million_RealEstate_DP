using MongoDB.Bson;
using RealEstateAPI.Domain.Entities;
using RealEstateAPI.Domain.Repositories;

namespace RealEstateAPI.Infrastructure.Data;

public class DatabaseSeeder
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IPropertyImageRepository _propertyImageRepository;
    private readonly IPropertyTraceRepository _propertyTraceRepository;

    public DatabaseSeeder(
        IOwnerRepository ownerRepository,
        IPropertyRepository propertyRepository,
        IPropertyImageRepository propertyImageRepository,
        IPropertyTraceRepository propertyTraceRepository)
    {
        _ownerRepository = ownerRepository;
        _propertyRepository = propertyRepository;
        _propertyImageRepository = propertyImageRepository;
        _propertyTraceRepository = propertyTraceRepository;
    }

    public async Task SeedAsync()
    {
        // Verificar si ya hay datos
        var existingOwners = await _ownerRepository.GetAllAsync();
        if (existingOwners.Any())
        {
            return; // Ya hay datos
        }

        // Crear propietarios de ejemplo
        var owners = new List<Owner>
        {
            new Owner
            {
                IdOwner = "OWN001",
                Name = "Carlos Rodríguez",
                Address = "Calle 123 #45-67, Bogotá",
                Photo = "https://via.placeholder.com/150/009688/FFFFFF?text=CR",
                Birthday = new DateTime(1980, 5, 15)
            },
            new Owner
            {
                IdOwner = "OWN002",
                Name = "María González",
                Address = "Carrera 15 #20-30, Medellín",
                Photo = "https://via.placeholder.com/150/E91E63/FFFFFF?text=MG",
                Birthday = new DateTime(1975, 8, 22)
            },
            new Owner
            {
                IdOwner = "OWN003",
                Name = "Juan Pérez",
                Address = "Avenida 6 #12-34, Cali",
                Photo = "https://via.placeholder.com/150/2196F3/FFFFFF?text=JP",
                Birthday = new DateTime(1985, 12, 10)
            }
        };

        foreach (var owner in owners)
        {
            await _ownerRepository.AddAsync(owner);
        }

        // Crear propiedades de ejemplo
        var properties = new List<Property>
        {
            new Property
            {
                IdProperty = "PROP001",
                Name = "Casa Familiar en La Zona Rosa",
                Address = "Carrera 13 #85-40, Zona Rosa, Bogotá",
                Price = 850000000m,
                CodeInternal = "ZR-001",
                Year = 2020,
                IdOwner = "OWN001"
            },
            new Property
            {
                IdProperty = "PROP002", 
                Name = "Apartamento Moderno en El Poblado",
                Address = "Calle 10 #43A-30, El Poblado, Medellín",
                Price = 650000000m,
                CodeInternal = "EP-002",
                Year = 2019,
                IdOwner = "OWN002"
            },
            new Property
            {
                IdProperty = "PROP003",
                Name = "Penthouse con Vista al Mar",
                Address = "Avenida Colombia #15-25, Cali",
                Price = 1200000000m,
                CodeInternal = "VM-003",
                Year = 2021,
                IdOwner = "OWN003"
            },
            new Property
            {
                IdProperty = "PROP004",
                Name = "Casa Campestre en Chía",
                Address = "Kilómetro 5 Vía Chía, Cundinamarca",
                Price = 450000000m,
                CodeInternal = "CH-004",
                Year = 2018,
                IdOwner = "OWN001"
            },
            new Property
            {
                IdProperty = "PROP005",
                Name = "Oficina Empresarial Centro",
                Address = "Carrera 7 #32-16, Centro, Bogotá",
                Price = 320000000m,
                CodeInternal = "CE-005",
                Year = 2017,
                IdOwner = "OWN002"
            }
        };

        foreach (var property in properties)
        {
            await _propertyRepository.AddAsync(property);
        }

        // Crear imágenes de ejemplo
        var images = new List<PropertyImage>
        {
            new PropertyImage { IdPropertyImage = "IMG001", IdProperty = "PROP001", File = "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG002", IdProperty = "PROP001", File = "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG003", IdProperty = "PROP002", File = "https://images.unsplash.com/photo-1600607688960-5f8d18a8a8e6?w=800", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG004", IdProperty = "PROP003", File = "https://images.unsplash.com/photo-1600607688969-a5bfcd646154?w=800", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG005", IdProperty = "PROP004", File = "https://images.unsplash.com/photo-1600607688920-4e2a09cf159d?w=800", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG006", IdProperty = "PROP005", File = "https://images.unsplash.com/photo-1600607688943-1450b9278533?w=800", Enabled = true }
        };

        foreach (var image in images)
        {
            await _propertyImageRepository.AddAsync(image);
        }

        // Crear trazas de ejemplo
        var traces = new List<PropertyTrace>
        {
            new PropertyTrace { IdPropertyTrace = "TRC001", IdProperty = "PROP001", DateSale = DateTime.Now.AddDays(-30), Name = "Venta Inicial", Value = 850000000m, Tax = 34000000m },
            new PropertyTrace { IdPropertyTrace = "TRC002", IdProperty = "PROP002", DateSale = DateTime.Now.AddDays(-60), Name = "Venta Inicial", Value = 650000000m, Tax = 26000000m },
            new PropertyTrace { IdPropertyTrace = "TRC003", IdProperty = "PROP003", DateSale = DateTime.Now.AddDays(-15), Name = "Venta Inicial", Value = 1200000000m, Tax = 48000000m },
            new PropertyTrace { IdPropertyTrace = "TRC004", IdProperty = "PROP004", DateSale = DateTime.Now.AddDays(-90), Name = "Venta Inicial", Value = 450000000m, Tax = 18000000m },
            new PropertyTrace { IdPropertyTrace = "TRC005", IdProperty = "PROP005", DateSale = DateTime.Now.AddDays(-45), Name = "Venta Inicial", Value = 320000000m, Tax = 12800000m }
        };

        foreach (var trace in traces)
        {
            await _propertyTraceRepository.AddAsync(trace);
        }
    }
}