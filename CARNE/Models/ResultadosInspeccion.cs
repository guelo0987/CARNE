using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class ResultadosInspeccion
{
    public int IdResultado { get; set; }

    public int IdInspeccion { get; set; }

    public int IdLista { get; set; }

    public int IdItem { get; set; }

    public bool Cumple { get; set; }

    public string? Observacion { get; set; }

    public virtual Inspeccione IdInspeccionNavigation { get; set; } = null!;

    public virtual ItemsVerificacion IdItemNavigation { get; set; } = null!;

    public virtual ListaVerificacion IdListaNavigation { get; set; } = null!;

    public virtual ICollection<Irregularidad> Irregularidads { get; set; } = new List<Irregularidad>();
}
