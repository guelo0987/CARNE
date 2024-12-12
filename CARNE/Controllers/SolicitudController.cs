using CARNE.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CARNE.Controllers;



[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SolicitudController:ControllerBase
{
    
    
    
    private readonly MyDbContext _db;

    public SolicitudController(MyDbContext db)
    {
        _db = db;
    }




    [HttpGet]
    public IActionResult GetSolicitudes()
    {

        var Solicitudes = _db.Solicituds.ToList();

        return Ok(Solicitudes);

    }


    
}