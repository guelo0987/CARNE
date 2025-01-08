using System.Linq;
using CARNE.Context;
using CARNE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CARNE.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "RequireAdministratorRole")]
public class ClienteController : ControllerBase
{
    private readonly MyDbContext _db;

    public ClienteController(MyDbContext db)
    {
        _db = db;
    }

    // Obtener todos los usuarios clientes
    [HttpGet]
    public IActionResult GetAllClientes()
    {
        var clientes = _db.Usuarios
            .Where(u => u.Rol == "Cliente")
            .ToList();

        if (!clientes.Any())
        {
            return NotFound("No hay clientes registrados.");
        }

        return Ok(clientes);
    }

    // Obtener el detalle de un cliente por ID
    [HttpGet("{id}")]
    public IActionResult GetClienteById(int id)
    {
        var cliente = _db.Usuarios
            .Include(u => u.Solicituds) // Incluye las solicitudes del cliente
            .FirstOrDefault(u => u.IdUsuario == id && u.Rol == "Cliente");

        if (cliente == null)
        {
            return NotFound("Cliente no encontrado.");
        }

        return Ok(cliente);
    }

    // Obtener las solicitudes realizadas por un cliente
    [HttpGet("{id}/solicitudes")]
    public IActionResult GetSolicitudesByCliente(int id)
    {
        var cliente = _db.Usuarios
            .Include(u => u.Solicituds)
            .FirstOrDefault(u => u.IdUsuario == id && u.Rol == "Cliente");

        if (cliente == null)
        {
            return NotFound("Cliente no encontrado.");
        }

        var solicitudes = cliente.Solicituds.ToList();

        if (!solicitudes.Any())
        {
            return NotFound("No hay solicitudes registradas para este cliente.");
        }

        return Ok(solicitudes);
    }

    // Cambiar el estado de la cuenta de un cliente
    [HttpPatch("{id}/cambiar-estado")]
    public IActionResult CambiarEstadoCliente(int id, [FromBody] string nuevoEstado)
    {
        if (string.IsNullOrEmpty(nuevoEstado) || (nuevoEstado != "Activo" && nuevoEstado != "Inactivo"))
        {
            return BadRequest("Estado inválido. Los estados permitidos son 'Activo' o 'Inactivo'.");
        }

        var cliente = _db.Usuarios.FirstOrDefault(u => u.IdUsuario == id && u.Rol == "Cliente");

        if (cliente == null)
        {
            return NotFound("Cliente no encontrado.");
        }

        cliente.Estado = nuevoEstado;
        _db.SaveChanges();

        return Ok(new { Message = $"El estado del cliente ha sido actualizado a '{nuevoEstado}'." });
    }
}
