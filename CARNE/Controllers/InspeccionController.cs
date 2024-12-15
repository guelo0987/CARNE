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
                IdSolicitud = inspeccionDto.IdSolicitud,
                IdAdmin = inspeccionDto.IdAdmin,
                IdAdminInspector = inspeccionDto.IdAdminInspector,
                FechaInspeccion = inspeccionDto.FechaInspeccion,
                Prioridad = inspeccionDto.Prioridad,
                Resultado = "En Revision"
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
    
 
    
    [HttpPost("CrearInspeccionAleatorio")]
    public IActionResult CrearInspeccionDesdeEstablecimiento()
    {
        var establecimientos = _db.Establecimientos.ToList();
        if (!establecimientos.Any())
        {
            return NotFound("No hay establecimientos registrados para generar una inspección.");
        }

        // Seleccionar un establecimiento aleatorio
        var random = new Random();
        var establecimientoSeleccionado = establecimientos[random.Next(establecimientos.Count)];

        // Seleccionar un administrador con el rol "Empleado"
        var empleados = _db.Admins.Where(a => a.Rol == "Empleado").ToList();
        if (!empleados.Any())
        {
            return NotFound("No hay administradores con el rol de 'Empleado' disponibles.");
        }

        var empleadoSeleccionado = empleados[random.Next(empleados.Count)];

        // Crear la inspección
        var nuevaInspeccion = new Inspeccione
        {
            IdEstablecimiento = establecimientoSeleccionado.IdEstablecimiento,
            IdAdminInspector = empleadoSeleccionado.IdAdmin, // Asignar un inspector aleatorio
            IdAdmin = null, // El administrador general será null
            FechaInspeccion = DateTime.Now,
            Prioridad = 1 // Asignar prioridad predeterminada
        };

        _db.Inspecciones.Add(nuevaInspeccion);
        _db.SaveChanges();

        return Ok(new
        {
            Mensaje = "Inspección creada exitosamente.",
            Inspeccion = nuevaInspeccion
        });
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
