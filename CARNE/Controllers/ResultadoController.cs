using CARNE.Context;
using CARNE.DTO;
using CARNE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CARNE.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ResultadoController : ControllerBase
{
    private readonly MyDbContext _db;

    public ResultadoController(MyDbContext db)
    {
        _db = db;
    }

    // GET: api/Resultado
    [HttpGet]
    public IActionResult GetResultados()
    {
        var resultados = _db.ResultadosInspeccions
            .Include(r => r.IdInspeccionNavigation)
            .Include(r => r.IdListaNavigation)
            .Include(r => r.IdItemNavigation)
            .ToList();

        if (resultados == null || !resultados.Any())
        {
            return NotFound("No hay resultados de inspección registrados.");
        }
        return Ok(resultados);
    }

    // GET: api/Resultado/{id}
    [HttpGet("{id}")]
    public IActionResult GetResultadoById(int id)
    {
        var resultado = _db.ResultadosInspeccions
            .Include(r => r.IdInspeccionNavigation)
            .Include(r => r.IdListaNavigation)
            .Include(r => r.IdItemNavigation)
            .FirstOrDefault(r => r.IdResultado == id);

        if (resultado == null)
        {
            return NotFound("Resultado de inspección no encontrado.");
        }
        return Ok(resultado);
    }

    // POST: api/Resultado
    //Crear un resultado de inspeccion por una solicitud
    [HttpPost]
    public IActionResult UpsertResultado([FromBody] ResultadoDTO resultadoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        // Verificar que el ítem pertenece a la lista especificada
        var itemValido = _db.ItemsVerificacions
            .Any(i => i.IdItem == resultadoDto.IdItem && i.IdLista == resultadoDto.IdLista);

        if (!itemValido)
        {
            return BadRequest("El ítem no pertenece a la lista especificada.");
        }


        var existingResultado = _db.ResultadosInspeccions
            .FirstOrDefault(r => r.IdResultado == resultadoDto.IdResultado);

        if (existingResultado == null)
        {
            // Create new record
            var newResultado = new ResultadosInspeccion
            {
                IdInspeccion = resultadoDto.IdInspeccion,
                IdLista = resultadoDto.IdLista,
                IdItem = resultadoDto.IdItem,
                Cumple = resultadoDto.Cumple,
                Observacion = resultadoDto.Observacion
            };

            _db.ResultadosInspeccions.Add(newResultado);
        }
        else
        {
            // Update existing record
            existingResultado.IdInspeccion = resultadoDto.IdInspeccion;
            existingResultado.IdLista = resultadoDto.IdLista;
            existingResultado.IdItem = resultadoDto.IdItem;
            existingResultado.Cumple = resultadoDto.Cumple;
            existingResultado.Observacion = resultadoDto.Observacion;
        }

        _db.SaveChanges();
        return Ok("Operación completada correctamente.");
    }

    

    
    
    
    
    
    
    
    
    //POST Finalizarinspeccion de Establecimiento Aleatorio, comienzo de irregularidades
    
    
    
    
    
    
    
    // POST: api/Resultado/FinalizarInspeccion
    [HttpPost("FinalizarInspeccionSolicitud")]
    public IActionResult FinalizarInspeccion(int idInspeccion)
    {
        var inspeccion = _db.Inspecciones
            .Include(i => i.IdSolicitudNavigation)
            .FirstOrDefault(i => i.IdInspeccion == idInspeccion);

        if (inspeccion == null)
        {
            return NotFound("Inspección no encontrada.");
        }

        var resultados = _db.ResultadosInspeccions
            .Where(r => r.IdInspeccion == idInspeccion)
            .ToList();

        if (!resultados.Any())
        {
            return BadRequest("No hay resultados asociados a esta inspección.");
        }

        var cumpleCount = resultados.Count(r => r.Cumple);
        var totalItems = resultados.Count;

        // Determinar si cumplió o no
        inspeccion.Resultado = cumpleCount > totalItems / 2 ? "Cumple" : "No Cumple";
        _db.SaveChanges();

        // Si cumplió, crear un nuevo establecimiento
        if (inspeccion.Resultado == "Cumple")
        {
            var solicitud = inspeccion.IdSolicitudNavigation;

            if (solicitud == null)
            {
                return BadRequest("No se encontró la solicitud asociada a esta inspección.");
            }

            // Crear establecimiento con los datos de la solicitud
            var nuevoEstablecimiento = new Establecimiento
            {
                Direccion = solicitud.Direccion,
                Nombre = solicitud.NombreEst,
                Riesgo = "Pendiente", // Atributos iniciales básicos
                TipoOperacion = solicitud.TipoOperacion,
                EstadoEstablecimiento = "Activo" // Estado inicial
            };

            _db.Establecimientos.Add(nuevoEstablecimiento);
            _db.SaveChanges();

            return Ok(new
            {
                Resultado = inspeccion.Resultado,
                Mensaje = "Inspección finalizada y establecimiento creado correctamente.",
                Establecimiento = nuevoEstablecimiento
            });
        }

        return Ok(new { Resultado = inspeccion.Resultado, Mensaje = "Inspección finalizada. No se creó un establecimiento porque no cumplió." });
    }


    // DELETE: api/Resultado/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteResultado(int id)
    {
        var resultado = _db.ResultadosInspeccions.Find(id);
        if (resultado == null)
        {
            return NotFound("Resultado de inspección no encontrado.");
        }

        _db.ResultadosInspeccions.Remove(resultado);
        _db.SaveChanges();

        return NoContent();
    }
}