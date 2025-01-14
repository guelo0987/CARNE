using CARNE.Context;
using CARNE.DTO;
using CARNE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CARNE.Controllers;



[ApiController]
[Route("api/Sancione")]
[Authorize]
public class SancioneController : ControllerBase
{


    private readonly MyDbContext _db;

    public SancioneController(MyDbContext db)
    {

        _db = db;

    }
    
    
    
    
    
    // PUT: api/Sancione/CambiarEstado/{idIrregularidad}/{idSancion}
    [HttpPut("CambiarEstado/{idIrregularidad}/{idSancion}")]
    public IActionResult CambiarEstadoSancion(int idIrregularidad, int idSancion, [FromBody] string nuevoEstado)
    {
        // Validar si la relación entre la irregularidad y la sanción existe
        var sancionIrregularidad = _db.SancionIrregularidads
            .FirstOrDefault(si => si.IdIrregularidad == idIrregularidad && si.IdSancion == idSancion);

        if (sancionIrregularidad == null)
        {
            return NotFound("No se encontró la relación entre la irregularidad y la sanción.");
        }

        // Cambiar el estado de la sanción
        sancionIrregularidad.EstadoSancion = nuevoEstado;

        // Registrar la fecha de resolución si el estado es "Resuelto"
        if (nuevoEstado.Equals("Resuelto", StringComparison.OrdinalIgnoreCase))
        {
            sancionIrregularidad.FechaResolution = DateTime.Now;
        }
        else
        {
            sancionIrregularidad.FechaResolution = null; // Limpiar la fecha si el estado no es "Resuelto"
        }

        // Guardar los cambios en la base de datos
        _db.SaveChanges();

        return Ok(new
        {
            Mensaje = "Estado de la sanción actualizado exitosamente.",
            SancionActualizada = new
            {
                sancionIrregularidad.IdIrregularidad,
                sancionIrregularidad.IdSancion,
                sancionIrregularidad.EstadoSancion,
                sancionIrregularidad.FechaResolution
            }
        });
    }

    
    
    


// GET: api/Sancione/{id}
    [HttpGet("{id}")]
    public IActionResult GetSancioneById(int id)
    {
        // Buscar la sanción por su ID
        var sancion = _db.Sanciones
            .Include(s => s.SancionIrregularidads) // Incluir las relaciones con irregularidades, si es necesario
            .FirstOrDefault(s => s.IdSancion == id);

        if (sancion == null)
        {
            return NotFound("Sanción no encontrada.");
        }

        // Preparar la respuesta
        var respuesta = new
        {
            sancion.IdSancion,
            sancion.Descripcion,
            sancion.Monto,
            IrregularidadesRelacionadas = sancion.SancionIrregularidads.Select(si => new
            {
                si.IdIrregularidad,
                si.FechaAplicada,
                si.FechaResolution,
                si.EstadoSancion
            }).ToList()
        };

        return Ok(respuesta);
    }



    [HttpGet]
    public IActionResult GetSancione()
    {

        return Ok(_db.Sanciones.Include(o=>o.SancionIrregularidads).ToList());
    }



    // POST: api/Irregularidad/AplicarSancionIrregularidad
    [HttpPost("AplicarSancioneIrregularidad")]
    public IActionResult AplicarSancionIrregularidad([FromBody] SancionIrregularidadDTO sancionIrregularidadDto)
    {
        // Validar la irregularidad
        var irregularidad = _db.Irregularidads
            .FirstOrDefault(i => i.IdIrregularidad == sancionIrregularidadDto.IdIrregularidad);

        if (irregularidad == null)
        {
            return NotFound("Irregularidad no encontrada.");
        }

        // Validar la sanción
        var sancion = _db.Sanciones
            .FirstOrDefault(s => s.IdSancion == sancionIrregularidadDto.IdSancion);

        if (sancion == null)
        {
            return NotFound("Sanción no encontrada.");
        }

        // Crear la relación entre irregularidad y sanción
        var nuevaSancionIrregularidad = new SancionIrregularidad
        {
            IdIrregularidad = sancionIrregularidadDto.IdIrregularidad,
            IdSancion = sancionIrregularidadDto.IdSancion,
            FechaAplicada = DateTime.Now,
            EstadoSancion = "Pendiente"
        };

        _db.SancionIrregularidads.Add(nuevaSancionIrregularidad);
        _db.SaveChanges();

        return Ok(new
        {
            Mensaje = "Sanción aplicada exitosamente a la irregularidad.",
            SancionIrregularidad = nuevaSancionIrregularidad
        });
    }
    
    
    
    
    
    //CREAR UNA SANCION 
    // POST: api/Sancione/Crear
    [HttpPost("Crear")]
    public IActionResult CrearSancion([FromBody] SancionDTO sancionDto)
    {
        if (sancionDto == null)
        {
            return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
        }

        if (string.IsNullOrEmpty(sancionDto.Descripcion))
        {
            return BadRequest("La descripción de la sanción es obligatoria.");
        }

        if (sancionDto.Monto <= 0)
        {
            return BadRequest("El monto de la sanción debe ser mayor que cero.");
        }

        // Crear una nueva sanción
        var nuevaSancion = new Sancione
        {
            Descripcion = sancionDto.Descripcion,
            Monto = sancionDto.Monto
        };

        // Agregar la sanción a la base de datos
        _db.Sanciones.Add(nuevaSancion);
        _db.SaveChanges();

        return Ok(new
        {
            Mensaje = "Sanción creada exitosamente.",
            SancionCreada = nuevaSancion
        });
    }




}