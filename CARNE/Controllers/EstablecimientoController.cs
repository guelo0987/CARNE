using CARNE.Context;
using CARNE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CARNE.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "RequireAdministratorRole")]
public class EstablecimientoController : ControllerBase
{
    private readonly MyDbContext _db;

    public EstablecimientoController(MyDbContext db)
    {
        _db = db;
    }

    // GET: api/Establecimiento
    [HttpGet]
    public IActionResult GetEstablecimientos()
    {
        var establecimientos = _db.Establecimientos.ToList();

        if (!establecimientos.Any())
        {
            return NotFound("No hay establecimientos registrados.");
        }

        return Ok(establecimientos);
    }

    // GET: api/Establecimiento/{id}
    [HttpGet("{id}")]
    public IActionResult GetEstablecimientoById(int id)
    {
        var establecimiento = _db.Establecimientos.Find(id);

        if (establecimiento == null)
        {
            return NotFound("Establecimiento no encontrado.");
        }

        return Ok(establecimiento);
    }

    // POST: api/Establecimiento
    [HttpPost]
    public IActionResult CreateEstablecimiento([FromBody] Establecimiento establecimiento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _db.Establecimientos.Add(establecimiento);
        _db.SaveChanges();

        return CreatedAtAction(nameof(GetEstablecimientoById), new { id = establecimiento.IdEstablecimiento }, establecimiento);
    }

    // PUT: api/Establecimiento/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateEstablecimiento(int id, [FromBody] Establecimiento establecimiento)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingEstablecimiento = _db.Establecimientos.Find(id);

        if (existingEstablecimiento == null)
        {
            return NotFound("Establecimiento no encontrado.");
        }

        existingEstablecimiento.Nombre = establecimiento.Nombre;
        existingEstablecimiento.Direccion = establecimiento.Direccion;
        existingEstablecimiento.Comerciales = establecimiento.Comerciales;
        existingEstablecimiento.TipoOperacion = establecimiento.TipoOperacion;
        existingEstablecimiento.TipoProducto = establecimiento.TipoProducto;
        existingEstablecimiento.CapacidadOperativa = establecimiento.CapacidadOperativa;
        existingEstablecimiento.VolumenProcesado = establecimiento.VolumenProcesado;
        existingEstablecimiento.UnidadVolumen = establecimiento.UnidadVolumen;
        existingEstablecimiento.PeriodoVolumen = establecimiento.PeriodoVolumen;
        existingEstablecimiento.Riesgo = establecimiento.Riesgo;
        existingEstablecimiento.LicenciasCertificaciones = establecimiento.LicenciasCertificaciones;
        existingEstablecimiento.EstadoEstablecimiento = establecimiento.EstadoEstablecimiento;

        _db.SaveChanges();

        return NoContent();
    }

    // DELETE: api/Establecimiento/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteEstablecimiento(int id)
    {
        var establecimiento = _db.Establecimientos.Find(id);

        if (establecimiento == null)
        {
            return NotFound("Establecimiento no encontrado.");
        }

        _db.Establecimientos.Remove(establecimiento);
        _db.SaveChanges();

        return NoContent();
    }

    // GET: api/Establecimiento/Search/{nombre}
    [HttpGet("Search/{nombre}")]
    public IActionResult SearchEstablecimiento(string nombre)
    {
        var establecimientos = _db.Establecimientos
            .Where(e => e.Nombre.Contains(nombre))
            .ToList();

        if (!establecimientos.Any())
        {
            return NotFound("No se encontraron establecimientos con ese nombre.");
        }

        return Ok(establecimientos);
    }
}
