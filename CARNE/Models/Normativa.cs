using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class Normativa
{
    public int IdNormativa { get; set; }

    public string NombreNormativa { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Version { get; set; }

    public DateTime? FechaAdmision { get; set; }

    public DateTime? FechaVigencia { get; set; }

    public virtual ICollection<ListaVerificacion> ListaVerificacions { get; set; } = new List<ListaVerificacion>();
}
