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
public class AnimalController : ControllerBase
{
    private readonly MyDbContext _db;

    public AnimalController(MyDbContext db)
    {
        _db = db;
    }

    // GET: api/Animal
    [HttpGet]
    public IActionResult GetAnimales()
    {
        var animales = _db.Animales.ToList();

        if (animales == null || !animales.Any())
        {
            return NotFound("No hay animales registrados.");
        }
        return Ok(animales);
    }

    // GET: api/Animal/{id}
    [HttpGet("{id}")]
    public IActionResult GetAnimalById(int id)
    {
        var animal = _db.Animales.Find(id);

        if (animal == null)
        {
            return NotFound("Animal no encontrado.");
        }
        return Ok(animal);
    }

    // UPSERT: api/Animal
    [HttpPost]
    public IActionResult UpsertAnimal([FromBody] AnimalDTO animalDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingAnimal = _db.Animales
            .FirstOrDefault(a => a.IdAnimal == animalDto.IdAnimal);

        if (existingAnimal == null)
        {
            // Create new record
            var newAnimal = new Animale()
            {
                IdEstablecimientoSacrificio = animalDto.IdEstablecimientoSacrificio,
                IdentificacionAnimal = animalDto.IdentificacionAnimal,
                Especie = animalDto.Especie,
                FechaSacrificio = animalDto.FechaSacrificio
            };

            _db.Animales.Add(newAnimal);
        }
        else
        {
            // Update existing record
            existingAnimal.IdEstablecimientoSacrificio = animalDto.IdEstablecimientoSacrificio;
            existingAnimal.IdentificacionAnimal = animalDto.IdentificacionAnimal;
            existingAnimal.Especie = animalDto.Especie;
            existingAnimal.FechaSacrificio = animalDto.FechaSacrificio;
        }

        _db.SaveChanges();
        return Ok("Operación completada correctamente.");
    }

    // DELETE: api/Animal/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        var animal = _db.Animales.Find(id);
        if (animal == null)
        {
            return NotFound("Animal no encontrado.");
        }

        _db.Animales.Remove(animal);
        _db.SaveChanges();

        return NoContent();
    }
}
