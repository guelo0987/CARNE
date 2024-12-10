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

    // Crear o Editar un usuario
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

    // Verificar si es una actualización (ID presente y válido)
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

        // Actualizar contraseña si se envía una nueva
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
        // Validaciones para un nuevo registro
        if (_db.Usuarios.Any(a => a.Email == adminDto.Email))
        {
            _logger.LogError("El correo ya está registrado.");
            return BadRequest("El correo ya está registrado.");
        }

        if (adminDto.Rol != "Admin" && adminDto.Rol != "Empleado")
        {
            _logger.LogError("El rol debe ser Admin o Empleado.");
            return BadRequest("El rol debe ser 'Admin' o 'Empleado'.");
        }

        var newUser = new Admin()
        {
            Username = adminDto.Username,
            Nombre = adminDto.Nombre,
            Email = adminDto.Email,
            Password = PassHasher.HashPassword(adminDto.Password), // Hasheamos la contraseña
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

    // Eliminar un usuario
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _db.Admins.FirstOrDefault(u => u.IdAdmin == id);

        if (user == null)
        {
            _logger.LogError($"Usuario con ID {id} no encontrado.");
            return NotFound("Usuario no encontrado.");
        }

        _db.Admins.Remove(user);
        _db.SaveChanges();
        _logger.LogInformation($"Usuario con ID {id} eliminado exitosamente.");

        return Ok("Usuario eliminado exitosamente.");
    }
}
