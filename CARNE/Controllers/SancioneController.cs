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




    [HttpGet]
    public IActionResult GetSancione()
    {

        return Ok(_db.Sanciones.ToList());
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



}