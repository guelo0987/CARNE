namespace CARNE.DTO;

public class SolicitudDTO
{
    public int IdSolicitud { get; set; }

    public int IdUsuarioCliente { get; set; }

    public DateTime? FechaAdmitida { get; set; }

    public DateTime? FechaAprobada { get; set; }

    public string? EstadoSolicitud { get; set; }
    
    public string Direccion { get; set; }
    
    public string NombreEst { get; set; }
    
    public string Coordenadas { get; set; }
    
    public string TipoOperacion { get; set; }
}