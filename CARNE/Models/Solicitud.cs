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

    public virtual Usuario IdUsuarioClienteNavigation { get; set; } = null!;

    public virtual ICollection<Inspeccione> Inspecciones { get; set; } = new List<Inspeccione>();
}
