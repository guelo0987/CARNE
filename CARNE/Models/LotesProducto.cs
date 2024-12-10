using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class LotesProducto
{
    public int IdLote { get; set; }

    public int IdEstablecimiento { get; set; }

    public string CodigoLote { get; set; } = null!;

    public DateTime? FechaProduccion { get; set; }

    public string? DescripcionProducto { get; set; }

    public string? DestinoFinal { get; set; }

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    public virtual Establecimiento IdEstablecimientoNavigation { get; set; } = null!;

    public virtual ICollection<Irregularidad> Irregularidads { get; set; } = new List<Irregularidad>();

    public virtual ICollection<Animale> IdAnimals { get; set; } = new List<Animale>();
}
