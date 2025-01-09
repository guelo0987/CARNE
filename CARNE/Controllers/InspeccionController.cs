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
    
    
    [HttpPost]
public IActionResult UpsertInspeccion([FromBody] InspeccionDTO inspeccionDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // Validar si el IdAdminInspector es válido
    var inspector = _db.Admins.FirstOrDefault(a => a.IdAdmin == inspeccionDto.IdAdminInspector && a.Rol == "Empleado");
    
    if (inspector == null)
    {
        return BadRequest("El administrador inspector no es válido o no existe.");
    }

    if (inspeccionDto.IdInspeccion <= 0)
    {
        // Crear nuevo registro
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
        // Buscar inspección existente
        var existingInspeccion = _db.Inspecciones.FirstOrDefault(u => u.IdInspeccion == inspeccionDto.IdInspeccion);

        if (existingInspeccion == null)
        {
            return NotFound("No se encontró la inspección.");
        }

        // Validar que la inspección no esté finalizada
        if (existingInspeccion.Resultado != "En Revision")
        {
            return BadRequest("No se pueden modificar inspecciones ya finalizadas.");
        }

        // Actualizar el registro existente
        existingInspeccion.IdSolicitud = inspeccionDto.IdSolicitud;
        existingInspeccion.IdAdmin = inspeccionDto.IdAdmin;
        existingInspeccion.IdAdminInspector = inspeccionDto.IdAdminInspector;
        existingInspeccion.FechaInspeccion = inspeccionDto.FechaInspeccion;
        existingInspeccion.Prioridad = inspeccionDto.Prioridad;
        existingInspeccion.Resultado = inspeccionDto.Resultado;
    }

    try
    {
        _db.SaveChanges();
        return Ok("Operación completada correctamente.");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error al guardar los cambios: {ex.Message}");
    }
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

    
    
    
    [HttpGet]
    public IActionResult GetInspecciones(
        [FromQuery] DateTime? fecha, 
        [FromQuery] string? direccion, 
        [FromQuery] string? resultado, 
        [FromQuery] int? prioridad, 
        [FromQuery] int? idAdminInspector)
    {
        IQueryable<Inspeccione> query = _db.Inspecciones;

        if (fecha.HasValue)
        {
            query = query.Where(i => i.FechaInspeccion.HasValue && 
                                     EF.Functions.DateDiffDay(i.FechaInspeccion.Value.Date, fecha.Value.Date) == 0);
        }

       

        if (!string.IsNullOrEmpty(resultado))
        {
            query = query.Where(i => i.Resultado != null && 
                                     EF.Functions.Like(i.Resultado, resultado));
        }

        if (prioridad.HasValue)
        {
            query = query.Where(i => i.Prioridad == prioridad.Value);
        }

        if (idAdminInspector.HasValue)
        {
            query = query.Where(i => i.IdAdminInspector == idAdminInspector.Value);
        }

        var inspecciones = query.ToListAsync();

        if (!inspecciones.Result.Any())
        {
            return NotFound("No se encontraron inspecciones que cumplan con los filtros proporcionados.");
        }

        return Ok(inspecciones.Result);
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
        if (inspeccion == null || inspeccion.Resultado != "En Revision")
        {
            return NotFound("No se pudo completar la operación");
        }

        _db.Inspecciones.Remove(inspeccion);
        _db.SaveChanges();

        return NoContent();
    }

    
}
