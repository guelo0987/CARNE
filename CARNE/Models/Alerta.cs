using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class Alerta
{
    public int IdAlerta { get; set; }

    public int IdIrregularidad { get; set; }

    public DateTime? FechaGenerada { get; set; }

    public string? Mensaje { get; set; }

    public string? Destinatarios { get; set; }

    public string? EstadoAlerta { get; set; }

    public virtual Irregularidad IdIrregularidadNavigation { get; set; } = null!;
}
