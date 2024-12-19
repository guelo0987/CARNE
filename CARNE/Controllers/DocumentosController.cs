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
public class DocumentoController : ControllerBase
{
    private readonly MyDbContext _db;

    public DocumentoController(MyDbContext db)
    {
        _db = db;
    }

    // GET: api/Documento
    [HttpGet]
    public IActionResult GetDocumentos()
    {
        var documentos = _db.Documentos.ToList();

        if (documentos == null || !documentos.Any())
        {
            return NotFound("No hay documentos registrados.");
        }
        return Ok(documentos);
    }

    // GET: api/Documento/{id}
    [HttpGet("{id}")]
    public IActionResult GetDocumentoById(int id)
    {
        var documento = _db.Documentos.Find(id);

        if (documento == null)
        {
            return NotFound("Documento no encontrado.");
        }
        return Ok(documento);
    }

    // UPSERT: api/Documento
    [HttpPost]
    public IActionResult UpsertDocumento([FromBody] DocumentoDTO documentoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingDocumento = _db.Documentos
            .FirstOrDefault(d => d.IdDocumento == documentoDto.IdDocumento);

        if (existingDocumento == null)
        {
            // Create new record
            var newDocumento = new Documento
            {
                IdLote = documentoDto.IdLote,
                TipoDocumento = documentoDto.TipoDocumento,
                NumeroDocumento = documentoDto.NumeroDocumento,
                FechaEmision = documentoDto.FechaEmision,
                FechaVencimiento = documentoDto.FechaVencimiento
            };

            _db.Documentos.Add(newDocumento);
        }
        else
        {
            // Update existing record
            existingDocumento.IdLote = documentoDto.IdLote;
            existingDocumento.TipoDocumento = documentoDto.TipoDocumento;
            existingDocumento.NumeroDocumento = documentoDto.NumeroDocumento;
            existingDocumento.FechaEmision = documentoDto.FechaEmision;
            existingDocumento.FechaVencimiento = documentoDto.FechaVencimiento;
        }

        _db.SaveChanges();
        return Ok("Operación completada correctamente.");
    }

    // DELETE: api/Documento/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteDocumento(int id)
    {
        var documento = _db.Documentos.Find(id);
        if (documento == null)
        {
            return NotFound("Documento no encontrado.");
        }

        _db.Documentos.Remove(documento);
        _db.SaveChanges();

        return NoContent();
    }
}
