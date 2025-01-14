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
    
[HttpGet("PorInspeccion/{idInspeccion}")]
public IActionResult GetIrregularidadesPorInspeccion(int idInspeccion)
{
    // Validar si la inspección existe
    var inspeccion = _db.Inspecciones
        .Include(i => i.IdAdminNavigation) // Incluir la navegación del administrador
        .Include(i => i.IdEstablecimientoNavigation) // Incluir el establecimiento relacionado
        .FirstOrDefault(i => i.IdInspeccion == idInspeccion);

    if (inspeccion == null)
    {
        return NotFound("Inspección no encontrada.");
    }

    // Obtener el nombre del inspector directamente
    var inspectorNombre = _db.Admins
        .Where(a => a.IdAdmin == inspeccion.IdAdminInspector)
        .Select(a => a.Nombre)
        .FirstOrDefault();

    // Obtener las irregularidades asociadas a la inspección
    var irregularidades = _db.Irregularidads
        .Include(i => i.IdResultadoInspeccionNavigation) // Incluir resultados relacionados
        .Include(i => i.IdLoteNavigation) // Incluir lotes relacionados
        .Include(i => i.IdEstablecimientoNavigation) // Incluir establecimiento relacionado
        .Where(i => i.IdResultadoInspeccionNavigation.IdInspeccion == idInspeccion)
        .Select(i => new
        {
            IdIrregularidad = i.IdIrregularidad,
            Descripcion = i.DescripcionIrregularidad,
            Tipo = i.Tipo,
            FechaDetectada = i.FechaDetectada,
            NivelGravedad = i.NivelGravedad,
            Establecimiento = i.IdEstablecimientoNavigation != null
                ? new
                {
                    i.IdEstablecimientoNavigation.IdEstablecimiento,
                    i.IdEstablecimientoNavigation.Nombre,
                    i.IdEstablecimientoNavigation.Direccion
                }
                : null,
            Lote = i.IdLoteNavigation != null
                ? new
                {
                    i.IdLoteNavigation.IdLote,
                    i.IdLoteNavigation.CodigoLote,
                    i.IdLoteNavigation.FechaProduccion,
                    i.IdLoteNavigation.DescripcionProducto,
                    i.IdLoteNavigation.DestinoFinal
                }
                : null
        })
        .ToList();

    // Preparar la respuesta con detalles de la inspección y las irregularidades
    var respuesta = new
    {
        CodigoInspeccion = $"IN-{idInspeccion}",
        FechaInspeccion = inspeccion.FechaInspeccion?.ToString("dd/MM/yyyy") ?? "Fecha no asignada",
        Establecimiento = inspeccion.IdEstablecimientoNavigation?.Nombre ?? "No asignado",
        Inspector = inspectorNombre ?? "No asignado",
        ListaDeIrregularidades = irregularidades
    };

    return Ok(respuesta);
}


        
  
    [HttpPut("EditarIrregularidad")]
    public IActionResult EditarIrregularidad(int idIrregularidad, int idLote, string? NivelGravedad)
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
        irregularidad.NivelGravedad = NivelGravedad;
        _db.SaveChanges();

        return Ok(new
        {
            Mensaje = "Irregularidad actualizada con éxito.",
            Irregularidad = irregularidad
        });
    }
    // GET: api/Irregularidad/TieneIrregularidadesInspeccion
    [HttpGet("TieneIrregularidadesInspeccion")]
    public IActionResult TieneIrregularidadesInspeccion(int idInspeccion)
    {
        // Validar si la inspección existe
        var inspeccion = _db.Inspecciones.FirstOrDefault(i => i.IdInspeccion == idInspeccion);
        if (inspeccion == null)
        {
            return NotFound("Inspección no encontrada.");
        }

        // Verificar si hay irregularidades asociadas a la inspección
        var tieneIrregularidades = _db.Irregularidads
            .Include(i => i.IdResultadoInspeccionNavigation)
            .Any(i => i.IdResultadoInspeccionNavigation.IdInspeccion == idInspeccion);

        // Retornar resultado
        return Ok(new
        {
            IdInspeccion = idInspeccion,
            TieneIrregularidades = tieneIrregularidades
        });
    }

    
    
    
    }