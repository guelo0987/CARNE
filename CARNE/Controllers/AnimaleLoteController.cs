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
public class AnimalesLotesController : ControllerBase
{
    private readonly MyDbContext _db;

    public AnimalesLotesController(MyDbContext db)
    {
        _db = db;
    }

    // GET: api/AnimalesLotes
    [HttpGet]
    public IActionResult GetAnimalesLotes()
    {
        var relaciones = _db.Animales
            .Include(a => a.IdLotes)
            .Select(a => new
            {
                AnimalId = a.IdAnimal,
                Lotes = a.IdLotes.Select(l => new
                {
                    LoteId = l.IdLote,
                    l.CodigoLote
                })
            })
            .ToList();

        if (!relaciones.Any())
        {
            return NotFound("No hay relaciones registradas.");
        }

        return Ok(relaciones);
    }

    // POST: api/AnimalesLotes
    [HttpPost]
    public IActionResult RelacionarAnimalLote([FromBody] AnimalesLoteDTO animalesLoteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Validar existencia de animal
        var animal = _db.Animales.Include(a => a.IdLotes).FirstOrDefault(a => a.IdAnimal == animalesLoteDto.IdAnimal);
        if (animal == null)
        {
            return NotFound("Animal no encontrado.");
        }

        // Validar existencia de lote
        var lote = _db.LotesProductos.FirstOrDefault(l => l.IdLote == animalesLoteDto.IdLote);
        if (lote == null)
        {
            return NotFound("Lote no encontrado.");
        }

        // Verificar si la relación ya existe
        if (animal.IdLotes.Contains(lote))
        {
            return BadRequest("La relación ya existe.");
        }

        // Crear la relación
        animal.IdLotes.Add(lote);
        _db.SaveChanges();

        return Ok("Relación creada exitosamente.");
    }

    // DELETE: api/AnimalesLotes
    [HttpDelete]
    public IActionResult EliminarRelacion([FromBody] AnimalesLoteDTO animalesLoteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Buscar la relación
        var animal = _db.Animales.Include(a => a.IdLotes).FirstOrDefault(a => a.IdAnimal == animalesLoteDto.IdAnimal);
        if (animal == null)
        {
            return NotFound("Animal no encontrado.");
        }

        var lote = animal.IdLotes.FirstOrDefault(l => l.IdLote == animalesLoteDto.IdLote);
        if (lote == null)
        {
            return NotFound("Relación no encontrada.");
        }

        // Eliminar la relación
        animal.IdLotes.Remove(lote);
        _db.SaveChanges();

        return Ok("Relación eliminada exitosamente.");
    }
}
