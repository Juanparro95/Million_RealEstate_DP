using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Application.Interfaces;
using RealEstateAPI.Application.DTOs;

namespace RealEstateAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(IPropertyService propertyService, ILogger<PropertiesController> logger)
    {
        _propertyService = propertyService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las propiedades con filtros opcionales
    /// </summary>
    /// <param name="name">Nombre de la propiedad (búsqueda parcial)</param>
    /// <param name="address">Dirección de la propiedad (búsqueda parcial)</param>
    /// <param name="minPrice">Precio mínimo</param>
    /// <param name="maxPrice">Precio máximo</param>
    /// <param name="page">Número de página (default: 1)</param>
    /// <param name="pageSize">Tamaño de página (default: 10)</param>
    /// <returns>Lista de propiedades</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropertyListDto>>> GetProperties(
        [FromQuery] string? name = null,
        [FromQuery] string? address = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Getting properties with filters: name={Name}, address={Address}, minPrice={MinPrice}, maxPrice={MaxPrice}, page={Page}, pageSize={PageSize}",
                name, address, minPrice, maxPrice, page, pageSize);

            var properties = await _propertyService.GetPropertiesAsync(name, address, minPrice, maxPrice, page, pageSize);
            
            _logger.LogInformation("Retrieved {Count} properties", properties.Count());
            
            return Ok(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting properties with filters: name={Name}, address={Address}, minPrice={MinPrice}, maxPrice={MaxPrice}",
                name, address, minPrice, maxPrice);
            return StatusCode(500, new { 
                message = "An error occurred while retrieving properties", 
                error = ex.Message,
                stackTrace = ex.StackTrace 
            });
        }
    }

    /// <summary>
    /// Obtiene una propiedad por su ID
    /// </summary>
    /// <param name="id">ID de la propiedad</param>
    /// <returns>Detalles de la propiedad</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyDto>> GetProperty(string id)
    {
        try
        {
            _logger.LogInformation("Getting property with ID: {Id}", id);
            
            var property = await _propertyService.GetPropertyDetailAsync(id);
            
            if (property == null)
            {
                _logger.LogWarning("Property with ID {Id} not found", id);
                return NotFound($"Property with ID {id} not found");
            }
            
            return Ok(property);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting property with ID: {Id}", id);
            return StatusCode(500, "An error occurred while retrieving the property");
        }
    }

    /// <summary>
    /// Obtiene propiedades de un propietario específico
    /// </summary>
    /// <param name="ownerId">ID del propietario</param>
    /// <returns>Lista de propiedades del propietario</returns>
    [HttpGet("owner/{ownerId}")]
    public async Task<ActionResult<IEnumerable<PropertyListDto>>> GetPropertiesByOwner(string ownerId)
    {
        try
        {
            _logger.LogInformation("Getting properties for owner ID: {OwnerId}", ownerId);
            
            var properties = await _propertyService.GetPropertiesByOwnerAsync(ownerId);
            
            return Ok(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting properties for owner ID: {OwnerId}", ownerId);
            return StatusCode(500, "An error occurred while retrieving properties");
        }
    }

    /// <summary>
    /// Crea una nueva propiedad
    /// </summary>
    /// <param name="request">Datos de la nueva propiedad</param>
    /// <returns>Propiedad creada</returns>
    [HttpPost]
    public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] CreatePropertyRequest request)
    {
        try
        {
            _logger.LogInformation("Creating new property: {Name}", request.Name);
            
            var createdProperty = await _propertyService.CreatePropertyAsync(request);
            
            return CreatedAtAction(nameof(GetProperty), new { id = createdProperty.Id }, createdProperty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating property");
            return StatusCode(500, "An error occurred while creating the property");
        }
    }

    /// <summary>
    /// Actualiza una propiedad existente
    /// </summary>
    /// <param name="id">ID de la propiedad</param>
    /// <param name="request">Datos actualizados</param>
    /// <returns>Propiedad actualizada</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PropertyDto>> UpdateProperty(string id, [FromBody] UpdatePropertyRequest request)
    {
        try
        {
            _logger.LogInformation("Updating property with ID: {Id}", id);
            
            var updatedProperty = await _propertyService.UpdatePropertyAsync(id, request);
            
            if (updatedProperty == null)
            {
                _logger.LogWarning("Property with ID {Id} not found for update", id);
                return NotFound($"Property with ID {id} not found");
            }
            
            return Ok(updatedProperty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating property with ID: {Id}", id);
            return StatusCode(500, "An error occurred while updating the property");
        }
    }

    /// <summary>
    /// Elimina una propiedad (soft delete)
    /// </summary>
    /// <param name="id">ID de la propiedad</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProperty(string id)
    {
        try
        {
            _logger.LogInformation("Deleting property with ID: {Id}", id);
            
            var result = await _propertyService.DeletePropertyAsync(id);
            
            if (!result)
            {
                _logger.LogWarning("Property with ID {Id} not found for deletion", id);
                return NotFound($"Property with ID {id} not found");
            }
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting property with ID: {Id}", id);
            return StatusCode(500, "An error occurred while deleting the property");
        }
    }
}