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
public class InspeccionController : ControllerBase
{
    private readonly MyDbContext _db;

    public InspeccionController(MyDbContext db)
    {
        _db = db;
    }
    
    
    // UPSERT: api/Inspeccion
    [HttpPost]
    public IActionResult UpsertInspeccion([FromBody] InspeccionDTO inspeccionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        
        if (inspeccionDto.IdInspeccion <= 0)
        {
            // Create new record
            var newInspeccion = new Inspeccione
            {
                IdEstablecimiento = inspeccionDto.IdEstablecimiento,
                IdSolicitud = inspeccionDto.IdSolicitud,
                IdAdmin = inspeccionDto.IdAdmin,
                IdAdminInspector = inspeccionDto.IdAdminInspector,
                FechaInspeccion = inspeccionDto.FechaInspeccion,
                Prioridad = inspeccionDto.Prioridad,
                Resultado = inspeccionDto.Resultado
            };

            _db.Inspecciones.Add(newInspeccion);
        }
        else
        {
            var existingInspeccion = _db.Inspecciones.FirstOrDefault(u => u.IdInspeccion == inspeccionDto.IdInspeccion);

            if (existingInspeccion == null)
            {
                return NotFound("No se encontro la inspeccion");
            }

            
            // Update existing record
            existingInspeccion.IdEstablecimiento = inspeccionDto.IdEstablecimiento;
            existingInspeccion.IdSolicitud = inspeccionDto.IdSolicitud;
            existingInspeccion.IdAdmin = inspeccionDto.IdAdmin;
            existingInspeccion.IdAdminInspector = inspeccionDto.IdAdminInspector;
            existingInspeccion.FechaInspeccion = inspeccionDto.FechaInspeccion;
            existingInspeccion.Prioridad = inspeccionDto.Prioridad;
            existingInspeccion.Resultado = inspeccionDto.Resultado;
        }

        _db.SaveChanges();
        return Ok("Operación completada correctamente.");
    }

    // GET: api/Inspeccion
    [HttpGet]
    public IActionResult GetInspecciones()
    {
        var inspecciones = _db.Inspecciones.ToList();

        if (inspecciones == null || !inspecciones.Any())
        {
            return NotFound("No hay ninguna inspección registrada.");
        }
        return Ok(inspecciones);
    }

    // GET: api/Inspeccion/{id}
    [HttpGet("{id}")]
    public IActionResult GetInspeccionById(int id)
    {
        var inspeccion = _db.Inspecciones.Find(id);

        if (inspeccion == null)
        {
            return NotFound("Inspección no encontrada.");
        }
        return Ok(inspeccion);
    }

    
    
    

    // DELETE: api/Inspeccion/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteInspeccion(int id)
    {
        var inspeccion = _db.Inspecciones.Find(id);
        if (inspeccion == null)
        {
            return NotFound("Inspección no encontrada.");
        }

        _db.Inspecciones.Remove(inspeccion);
        _db.SaveChanges();

        return NoContent();
    }

    
}
