using CARNE.Context;
using CARNE.DTO;
using CARNE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CARNE.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "RequireAdministratorRole")]
public class InspeccionController:ControllerBase

{

    private readonly MyDbContext _db;
    


    public InspeccionController(MyDbContext db)
    {

        _db = db;
    }
    
    [HttpGet]
    public IActionResult GetSolicitudes()
    {

        var Solicitudes = _db.Solicituds.ToList();

        if (Solicitudes == null)
        {
            return NotFound("No hay ninguna Solicitud");
        }
        return Ok(Solicitudes);

    }


    [HttpPost]
    public IActionResult PostSolicitud([FromBody] InspeccionDTO inspeccionDto)
    {
        
        
        
        var NewInspeccion = new Inspeccione()
        {
           IdEstablecimiento = inspeccionDto.IdEstablecimiento,
           IdSolicitud = inspeccionDto.IdSolicitud,
           IdAdmin = inspeccionDto.IdAdmin,
           IdAdminInspector = inspeccionDto.IdAdminInspector,
           FechaInspeccion = inspeccionDto.FechaInspeccion,
           Prioridad = inspeccionDto.Prioridad,
           Direccion = inspeccionDto.Direccion,
           Coordenadas = inspeccionDto.Coordenadas,
           Resultado = inspeccionDto.Resultado
            
            
            
            
            
        };
        
        
        _db.Inspecciones.Add(NewInspeccion);
        _db.SaveChanges();
        
        return Ok(NewInspeccion);

        
    }
    
    
}