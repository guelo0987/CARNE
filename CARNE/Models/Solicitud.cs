using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class Solicitud
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
    
    public string RNC { get; set; }

    public virtual Usuario IdUsuarioClienteNavigation { get; set; } = null!;

    public virtual ICollection<Inspeccione> Inspecciones { get; set; } = new List<Inspeccione>();
}
