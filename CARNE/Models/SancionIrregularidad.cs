using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class SancionIrregularidad
{
    public int IdIrregularidad { get; set; }

    public int IdSancion { get; set; }

    public DateTime? FechaAplicada { get; set; }

    public DateTime? FechaResolution { get; set; }

    public string? EstadoSancion { get; set; }

    public virtual Irregularidad IdIrregularidadNavigation { get; set; } = null!;

    public virtual Sancione IdSancionNavigation { get; set; } = null!;
}
