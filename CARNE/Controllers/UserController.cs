using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CARNE.Context;
using CARNE.DTO;
using CARNE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PassHash;

namespace CARNE.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "RequireAdministratorRole")]
public class UserController : ControllerBase
{
    private readonly MyDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, MyDbContext db, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _db = db;
    }

   [HttpPost]
public IActionResult AddOrUpdate([FromBody] AdminDTO adminDto)
{
    if (adminDto == null)
    {
        _logger.LogError("El cuerpo de la solicitud está vacío.");
        return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
    }

    if (string.IsNullOrEmpty(adminDto.Email))
    {
        _logger.LogError("El correo es obligatorio.");
        return BadRequest("El correo es obligatorio.");
    }

    if (string.IsNullOrEmpty(adminDto.Username))
    {
        _logger.LogError("El nombre de usuario es obligatorio.");
        return BadRequest("El nombre de usuario es obligatorio.");
    }

    // Validación de unicidad
    if (_db.Admins.Any(u => u.Email == adminDto.Email && u.IdAdmin != adminDto.AdminId))
    {
        _logger.LogError($"El correo {adminDto.Email} ya está registrado.");
        return BadRequest($"El correo {adminDto.Email} ya está registrado.");
    }

    if (_db.Admins.Any(u => u.Username == adminDto.Username && u.IdAdmin != adminDto.AdminId))
    {
        _logger.LogError($"El nombre de usuario {adminDto.Username} ya está en uso.");
        return BadRequest($"El nombre de usuario {adminDto.Username} ya está en uso.");
    }

    if (adminDto.AdminId > 0)
    {
        var existingUser = _db.Admins.FirstOrDefault(u => u.IdAdmin == adminDto.AdminId);

        if (existingUser == null)
        {
            _logger.LogError($"Usuario con ID {adminDto.AdminId} no encontrado.");
            return NotFound("Usuario no encontrado.");
        }

        // Actualizar los valores
        existingUser.Username = adminDto.Username;
        existingUser.Nombre = adminDto.Nombre;
        existingUser.Email = adminDto.Email;
        existingUser.Rol = adminDto.Rol;
        existingUser.Telefono = adminDto.Telefono;

        if (!string.IsNullOrEmpty(adminDto.Password))
        {
            existingUser.Password = PassHasher.HashPassword(adminDto.Password);
        }

        _db.SaveChanges();
        _logger.LogInformation($"Usuario con ID {adminDto.AdminId} actualizado exitosamente.");
        return Ok(new { Message = "Usuario actualizado exitosamente.", UserId = existingUser.IdAdmin });
    }
    else
    {
        if (adminDto.Rol != "Admin" && adminDto.Rol != "Empleado")
        {
            _logger.LogError("El rol debe ser 'Admin' o 'Empleado'.");
            return BadRequest("El rol debe ser 'Admin' o 'Empleado'.");
        }

        var newUser = new Admin()
        {
            Username = adminDto.Username,
            Nombre = adminDto.Nombre,
            Email = adminDto.Email,
            Password = PassHasher.HashPassword(adminDto.Password),
            Rol = adminDto.Rol,
            FechaIngreso = DateTime.Now,
            Telefono = adminDto.Telefono
        };

        _db.Admins.Add(newUser);
        _db.SaveChanges();
        _logger.LogInformation("Usuario creado exitosamente.");

        return Ok(new { Message = "Usuario registrado exitosamente.", UserId = newUser.IdAdmin });
    }
}


    // Obtener todos los usuarios
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _db.Admins.ToList();
        return Ok(users);
    }

    // Obtener un usuario por ID
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = _db.Admins.FirstOrDefault(u => u.IdAdmin == id);

        if (user == null)
        {
            _logger.LogError($"Usuario con ID {id} no encontrado.");
            return NotFound("Usuario no encontrado.");
        }

        return Ok(user);
    }

 
    // Cambiar estado de usuario a "Inactivo" en lugar de eliminarlo
    [HttpDelete("{id}")]
    public IActionResult SetUserInactive(int id)
    {
        var user = _db.Admins.FirstOrDefault(u => u.IdAdmin == id);

        if (user == null)
        {
            _logger.LogError($"Usuario con ID {id} no encontrado.");
            return NotFound("Usuario no encontrado.");
        }

        // Cambiar el estado a "Inactivo"
        user.Estado = "Inactivo";
        _db.SaveChanges();
        _logger.LogInformation($"Estado del usuario con ID {id} cambiado a 'Inactivo'.");

        return Ok(new { Message = "El usuario ha sido desactivado exitosamente." });
    }

}
