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
public class LoteProductoController : ControllerBase
{
    private readonly MyDbContext _db;

    public LoteProductoController(MyDbContext db)
    {
        _db = db;
    }

    // GET: api/LoteProducto
    [HttpGet]
    public IActionResult GetLotesProducto()
    {
        var lotesProducto = _db.LotesProductos.ToList();

        if (lotesProducto == null || !lotesProducto.Any())
        {
            return NotFound("No hay lotes de producto registrados.");
        }
        return Ok(lotesProducto);
    }

    // GET: api/LoteProducto/{id}
    [HttpGet("{id}")]
    public IActionResult GetLoteProductoById(int id)
    {
        var loteProducto = _db.LotesProductos.Find(id);

        if (loteProducto == null)
        {
            return NotFound("Lote de producto no encontrado.");
        }
        return Ok(loteProducto);
    }

    // UPSERT: api/LoteProducto
    [HttpPost]
    public IActionResult UpsertLoteProducto([FromBody] LoteProductoDTO loteProductoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingLoteProducto = _db.LotesProductos
            .FirstOrDefault(l => l.IdLote == loteProductoDto.IdLote);

        if (existingLoteProducto == null)
        {
            // Create new record
            var newLoteProducto = new LotesProducto()
            {
                IdEstablecimiento = loteProductoDto.IdEstablecimiento,
                CodigoLote = loteProductoDto.CodigoLote,
                FechaProduccion = loteProductoDto.FechaProduccion,
                DescripcionProducto = loteProductoDto.DescripcionProducto,
                DestinoFinal = loteProductoDto.DestinoFinal
            };

            _db.LotesProductos.Add(newLoteProducto);
        }
        else
        {
            // Update existing record
            existingLoteProducto.IdEstablecimiento = loteProductoDto.IdEstablecimiento;
            existingLoteProducto.CodigoLote = loteProductoDto.CodigoLote;
            existingLoteProducto.FechaProduccion = loteProductoDto.FechaProduccion;
            existingLoteProducto.DescripcionProducto = loteProductoDto.DescripcionProducto;
            existingLoteProducto.DestinoFinal = loteProductoDto.DestinoFinal;
        }

        _db.SaveChanges();
        return Ok("Operación completada correctamente.");
    }

    // DELETE: api/LoteProducto/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteLoteProducto(int id)
    {
        var loteProducto = _db.LotesProductos.Find(id);
        if (loteProducto == null)
        {
            return NotFound("Lote de producto no encontrado.");
        }

        _db.LotesProductos.Remove(loteProducto);
        _db.SaveChanges();

        return NoContent();
    }
}
