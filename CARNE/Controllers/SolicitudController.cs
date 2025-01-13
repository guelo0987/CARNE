using CARNE.Context;
using CARNE.DTO;
using CARNE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CARNE.Controllers;



[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SolicitudController:ControllerBase
{
    
    
    
    private readonly MyDbContext _db;

    public SolicitudController(MyDbContext db)
    {
        _db = db;
    }




    [HttpGet]
    public IActionResult GetSolicitudes()
    {

        var Solicitudes = _db.Solicituds.Include(e=>e.Inspecciones).Include(u=>u.IdUsuarioClienteNavigation).ToList();

        return Ok(Solicitudes);

    }
    
    [HttpGet("{id}")]
    public IActionResult GetSolicitud(int id)
    {
        // Incluimos las relaciones necesarias (ej. IdUsuarioClienteNavigation) y buscamos por Id
        var solicitud = _db.Solicituds
            .Include(u => u.IdUsuarioClienteNavigation)
            .FirstOrDefault(s => s.IdSolicitud == id);

        // Verificamos si existe la solicitud
        if (solicitud == null)
        {
            return NotFound();
        }

        // Retornamos la solicitud encontrada
        return Ok(solicitud);
    }

    
    
    // PUT: api/Solicitud/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateSolicitud(int id, [FromBody] SolicitudDTO solicitudDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingSolicitud = _db.Solicituds.Include(s => s.IdUsuarioClienteNavigation).FirstOrDefault(s => s.IdSolicitud == id);

        if (existingSolicitud == null)
        {
            return NotFound("Solicitud no encontrada.");
        }

        // Actualizar los campos de la solicitud
        existingSolicitud.FechaAdmitida = solicitudDto.FechaAdmitida;
        existingSolicitud.FechaAprobada = solicitudDto.FechaAprobada;
        existingSolicitud.EstadoSolicitud = solicitudDto.EstadoSolicitud;
        existingSolicitud.Direccion = solicitudDto.Direccion;
        existingSolicitud.NombreEst = solicitudDto.NombreEst;
        existingSolicitud.Coordenadas = solicitudDto.Coordenadas;
        existingSolicitud.TipoOperacion = solicitudDto.TipoOperacion;

        try
        {
            _db.SaveChanges();
            return Ok(new { Message = "Solicitud actualizada correctamente.", Solicitud = existingSolicitud });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al actualizar la solicitud: {ex.Message}");
        }
    }
    


    
}