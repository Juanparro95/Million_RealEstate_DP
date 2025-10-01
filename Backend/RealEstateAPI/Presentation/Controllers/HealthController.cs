using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RealEstateAPI.Infrastructure.Data;

namespace RealEstateAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly MongoDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(MongoDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Verificar el estado de la API y la conexión a MongoDB
    /// </summary>
    /// <returns>Estado de salud del sistema</returns>
    [HttpGet]
    public async Task<ActionResult> GetHealth()
    {
        try
        {
            _logger.LogInformation("Health check requested");

            // Verificar conexión a MongoDB haciendo una consulta simple
            var ownersCount = await _context.Owners.CountDocumentsAsync(Builders<Domain.Entities.Owner>.Filter.Empty);

            var response = new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                database = new
                {
                    connected = true,
                    name = _context.Owners.Database.DatabaseNamespace.DatabaseName,
                    recordCount = ownersCount
                }
            };

            _logger.LogInformation("Health check successful. Database: {DatabaseName}, Owners: {Count}", 
                _context.Owners.Database.DatabaseNamespace.DatabaseName, ownersCount);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");

            var response = new
            {
                status = "unhealthy",
                timestamp = DateTime.UtcNow,
                error = ex.Message,
                database = new
                {
                    connected = false
                }
            };

            return StatusCode(503, response);
        }
    }

    /// <summary>
    /// Obtener estadísticas de la base de datos
    /// </summary>
    /// <returns>Estadísticas de colecciones</returns>
    [HttpGet("stats")]
    public async Task<ActionResult> GetStats()
    {
        try
        {
            var ownersCount = await _context.Owners.CountDocumentsAsync(Builders<Domain.Entities.Owner>.Filter.Empty);
            var propertiesCount = await _context.Properties.CountDocumentsAsync(Builders<Domain.Entities.Property>.Filter.Empty);
            var imagesCount = await _context.PropertyImages.CountDocumentsAsync(Builders<Domain.Entities.PropertyImage>.Filter.Empty);
            var tracesCount = await _context.PropertyTraces.CountDocumentsAsync(Builders<Domain.Entities.PropertyTrace>.Filter.Empty);

            var stats = new
            {
                database = _context.Owners.Database.DatabaseNamespace.DatabaseName,
                collections = new
                {
                    owners = ownersCount,
                    properties = propertiesCount,
                    propertyImages = imagesCount,
                    propertyTraces = tracesCount
                },
                timestamp = DateTime.UtcNow
            };

            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting database stats");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}