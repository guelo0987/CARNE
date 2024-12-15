using CARNE.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CARNE.Controllers;



[ApiController]
[Route("api/Irregularidad")]
[Authorize]
public class IrregularidadController:ControllerBase
{


    private readonly MyDbContext _db;
    
    public IrregularidadController(MyDbContext db)
    {

        _db = db;

    }
    
    // PUT: api/Resultado/EditarIrregularidad
    [HttpPut("EditarIrregularidad")]
    public IActionResult EditarIrregularidad(int idIrregularidad, int idLote)
    {
        // Buscar la irregularidad
        var irregularidad = _db.Irregularidads
            .Include(i => i.IdEstablecimientoNavigation)
            .FirstOrDefault(i => i.IdIrregularidad == idIrregularidad);

        if (irregularidad == null)
        {
            return NotFound("Irregularidad no encontrada.");
        }

        // Validar el establecimiento asociado
        var establecimiento = irregularidad.IdEstablecimientoNavigation;

        if (establecimiento == null || establecimiento.TipoOperacion != "Carnico")
        {
            return BadRequest("El establecimiento asociado no es del tipo 'Carnico' o no existe.");
        }

        // Validar que el lote pertenece al establecimiento
        var lote = _db.LotesProductos.FirstOrDefault(l => l.IdLote == idLote && l.IdEstablecimiento == establecimiento.IdEstablecimiento);

        if (lote == null)
        {
            return BadRequest("El lote no pertenece al establecimiento especificado.");
        }

        // Actualizar la irregularidad con el lote
        irregularidad.IdLote = idLote;
        _db.SaveChanges();

        return Ok(new
        {
            Mensaje = "Irregularidad actualizada con éxito.",
            Irregularidad = irregularidad
        });
    }
    
    
}