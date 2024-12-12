using CARNE.Context;
using CARNE.DTO;
using CARNE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CARNE.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "RequireAdministratorRole")]
public class NormativaController : ControllerBase
{
    private readonly MyDbContext _db;

    public NormativaController(MyDbContext db)
    {
        _db = db;
    }

    // GET: api/Normativa
    [HttpGet]
    public IActionResult GetNormativas()
    {
        var normativas = _db.Normativas.ToList();

        if (normativas == null || !normativas.Any())
        {
            return NotFound("No hay normativas registradas.");
        }
        return Ok(normativas);
    }

    // GET: api/Normativa/{id}
    [HttpGet("{id}")]
    public IActionResult GetNormativaById(int id)
    {
        var normativa = _db.Normativas.Find(id);

        if (normativa == null)
        {
            return NotFound("Normativa no encontrada.");
        }
        return Ok(normativa);
    }

    // UPSERT: api/Normativa
    [HttpPost]
    public IActionResult UpsertNormativa([FromBody] NormativaDTO normativaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingNormativa = _db.Normativas
            .FirstOrDefault(n => n.IdNormativa == normativaDto.IdNormativa);

        if (existingNormativa == null)
        {
            // Create new record
            var newNormativa = new Normativa
            {
                NombreNormativa = normativaDto.NombreNormativa,
                Descripcion = normativaDto.Descripcion,
                Version = normativaDto.Version,
                FechaAdmision = normativaDto.FechaAdmision,
                FechaVigencia = normativaDto.FechaVigencia
            };

            _db.Normativas.Add(newNormativa);
        }
        else
        {
            // Update existing record
            existingNormativa.NombreNormativa = normativaDto.NombreNormativa;
            existingNormativa.Descripcion = normativaDto.Descripcion;
            existingNormativa.Version = normativaDto.Version;
            existingNormativa.FechaAdmision = normativaDto.FechaAdmision;
            existingNormativa.FechaVigencia = normativaDto.FechaVigencia;
        }

        _db.SaveChanges();
        return Ok("Operación completada correctamente.");
    }

    // DELETE: api/Normativa/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteNormativa(int id)
    {
        var normativa = _db.Normativas.Find(id);
        if (normativa == null)
        {
            return NotFound("Normativa no encontrada.");
        }

        _db.Normativas.Remove(normativa);
        _db.SaveChanges();

        return NoContent();
    }
}
