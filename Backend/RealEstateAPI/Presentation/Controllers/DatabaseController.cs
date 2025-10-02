using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Infrastructure.Data;
using MongoDB.Driver;

namespace RealEstateAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly IMongoDbContext _context;
    private readonly DatabaseSeeder _seeder;

    public DatabaseController(IMongoDbContext context, DatabaseSeeder seeder)
    {
        _context = context;
        _seeder = seeder;
    }

    /// <summary>
    /// Limpia toda la base de datos (SOLO DESARROLLO)
    /// </summary>
    [HttpPost("clear")]
    public async Task<IActionResult> ClearDatabase()
    {
        try
        {
            // Eliminar todas las colecciones
            await _context.Owners.Database.DropCollectionAsync("Owners");
            await _context.Properties.Database.DropCollectionAsync("Properties");
            await _context.PropertyImages.Database.DropCollectionAsync("PropertyImages");
            await _context.PropertyTraces.Database.DropCollectionAsync("PropertyTraces");

            return Ok(new { message = "Base de datos limpiada exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    /// <summary>
    /// Reseeda toda la base de datos (SOLO DESARROLLO)
    /// </summary>
    [HttpPost("reseed")]
    public async Task<IActionResult> ReseedDatabase()
    {
        try
        {
            // Primero limpiar
            await _context.Owners.Database.DropCollectionAsync("Owners");
            await _context.Properties.Database.DropCollectionAsync("Properties");
            await _context.PropertyImages.Database.DropCollectionAsync("PropertyImages");
            await _context.PropertyTraces.Database.DropCollectionAsync("PropertyTraces");

            // Luego reseeder
            await _seeder.SeedAsync();

            return Ok(new { message = "Base de datos reseteada y reseedeada exitosamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
