namespace CARNE.DTO;

public class InspeccionDTO
{
    

    public int IdInspeccion { get; set; }
    

    public int IdSolicitud { get; set; }

    public int? IdAdmin { get; set; }
    
    public int IdAdminInspector { get; set; }
    
    public DateTime? FechaInspeccion { get; set; }

    public int? Prioridad { get; set; }

    public string? Direccion { get; set; }

    public string? Coordenadas { get; set; }

    public string? Resultado { get; set; }


}