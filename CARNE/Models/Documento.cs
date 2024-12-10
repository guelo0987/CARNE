using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class Documento
{
    public int IdDocumento { get; set; }

    public int IdLote { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public DateTime? FechaEmision { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public virtual LotesProducto IdLoteNavigation { get; set; } = null!;
}
