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
public class ListaVerificacionController : ControllerBase
{
    private readonly MyDbContext _db;

    public ListaVerificacionController(MyDbContext db)
    {
        _db = db;
    }

    // GET: api/ListaVerificacion
    [HttpGet]
    public IActionResult GetListasVerificacion()
    {
        var listas = _db.ListaVerificacions.Include(l => l.IdNormativaNavigation).ToList();

        if (listas == null || !listas.Any())
        {
            return NotFound("No hay listas de verificación registradas.");
        }
        return Ok(listas);
    }

    // GET: api/ListaVerificacion/{id}
    [HttpGet("{id}")]
    public IActionResult GetListaVerificacionById(int id)
    {
        var lista = _db.ListaVerificacions
            .Include(l => l.IdNormativaNavigation)
            .FirstOrDefault(l => l.IdLista == id);

        if (lista == null)
        {
            return NotFound("Lista de verificación no encontrada.");
        }
        return Ok(lista);
    }

    // UPSERT: api/ListaVerificacion
    [HttpPost]
    public IActionResult UpsertListaVerificacion([FromBody] ListaDTO listaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingLista = _db.ListaVerificacions
            .FirstOrDefault(l => l.IdLista == listaDto.IdLista);

        if (existingLista == null)
        {
            // Create new record
            var newLista = new ListaVerificacion
            {
                IdNormativa = listaDto.IdNormativa,
                NombreLista = listaDto.NombreLista,
                Descripcion = listaDto.Descripcion
            };

            _db.ListaVerificacions.Add(newLista);
        }
        else
        {
            // Update existing record
            existingLista.IdNormativa = listaDto.IdNormativa;
            existingLista.NombreLista = listaDto.NombreLista;
            existingLista.Descripcion = listaDto.Descripcion;
        }

        _db.SaveChanges();
        return Ok("Operación completada correctamente.");
    }

    // DELETE: api/ListaVerificacion/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteListaVerificacion(int id)
    {
        var lista = _db.ListaVerificacions.Find(id);
        if (lista == null)
        {
            return NotFound("Lista de verificación no encontrada.");
        }

        _db.ListaVerificacions.Remove(lista);
        _db.SaveChanges();

        return NoContent();
    }
}
