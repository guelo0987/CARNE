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
public class ItemsVerificacionController : ControllerBase
{
    private readonly MyDbContext _db;

    public ItemsVerificacionController(MyDbContext db)
    {
        _db = db;
    }

    // GET: api/ItemsVerificacion
    [HttpGet]
    public IActionResult GetItemsVerificacion()
    {
        var items = _db.ItemsVerificacions.Include(i => i.IdListaNavigation).ToList();

        if (items == null || !items.Any())
        {
            return NotFound("No hay ítems de verificación registrados.");
        }
        return Ok(items);
    }

    // GET: api/ItemsVerificacion/{id}
    [HttpGet("{id}")]
    public IActionResult GetItemVerificacionById(int id)
    {
        var item = _db.ItemsVerificacions
            .Include(i => i.IdListaNavigation)
            .FirstOrDefault(i => i.IdItem == id);

        if (item == null)
        {
            return NotFound("Ítem de verificación no encontrado.");
        }
        return Ok(item);
    }

    // GET: api/ItemsVerificacion/Lista/{idLista}
    [HttpGet("Lista/{idLista}")]
    public IActionResult GetItemsByList(int idLista)
    {
        var items = _db.ItemsVerificacions
            .Include(l => l.IdListaNavigation)
            .Where(l => l.IdLista == idLista)
            .ToList();

        if (items == null || !items.Any())
        {
            return NotFound("No hay ítems de verificación para la lista especificada.");
        }
        return Ok(items);
    }

    // UPSERT: api/ItemsVerificacion
    [HttpPost]
    public IActionResult UpsertItemVerificacion([FromBody] ItemsDTO itemDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingItem = _db.ItemsVerificacions
            .FirstOrDefault(i => i.IdItem == itemDto.IdItem);

        if (existingItem == null)
        {
            // Create new record
            var newItem = new ItemsVerificacion
            {
                IdLista = itemDto.IdLista,
                NumeroItem = itemDto.NumeroItem,
                Descripcion = itemDto.Descripcion,
                CriterioCumplimiento = itemDto.CriterioCumplimiento
            };

            _db.ItemsVerificacions.Add(newItem);
        }
        else
        {
            // Update existing record
            existingItem.IdLista = itemDto.IdLista;
            existingItem.NumeroItem = itemDto.NumeroItem;
            existingItem.Descripcion = itemDto.Descripcion;
            existingItem.CriterioCumplimiento = itemDto.CriterioCumplimiento;
        }

        _db.SaveChanges();
        return Ok("Operación completada correctamente.");
    }

    // DELETE: api/ItemsVerificacion/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteItemVerificacion(int id)
    {
        var item = _db.ItemsVerificacions.Find(id);
        if (item == null)
        {
            return NotFound("Ítem de verificación no encontrado.");
        }

        _db.ItemsVerificacions.Remove(item);
        _db.SaveChanges();

        return NoContent();
    }
}
