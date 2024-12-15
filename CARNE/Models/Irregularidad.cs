using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class Irregularidad
{
    public int IdIrregularidad { get; set; }

    public int IdEstablecimiento { get; set; }

    public int? IdLote { get; set; }

    public int IdResultadoInspeccion { get; set; }

    public string Tipo { get; set; } = null!;

    public DateTime? FechaDetectada { get; set; }

    public string? NivelGravedad { get; set; }

    public string? DescripcionIrregularidad { get; set; }

    public virtual ICollection<Alerta> Alerta { get; set; } = new List<Alerta>();

    public virtual Establecimiento IdEstablecimientoNavigation { get; set; } = null!;

    public virtual LotesProducto IdLoteNavigation { get; set; } = null!;

    public virtual ResultadosInspeccion IdResultadoInspeccionNavigation { get; set; } = null!;

    public virtual ICollection<SancionIrregularidad> SancionIrregularidads { get; set; } = new List<SancionIrregularidad>();
}
