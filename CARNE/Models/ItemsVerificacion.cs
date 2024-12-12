using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class ItemsVerificacion
{
    public int IdItem { get; set; }

    public int IdLista { get; set; }

    public int NumeroItem { get; set; }

    public string Descripcion { get; set; } = null!;

    public string? CriterioCumplimiento { get; set; }

    public virtual ListaVerificacion IdListaNavigation { get; set; } = null!;

    public virtual ICollection<ResultadosInspeccion> ResultadosInspeccions { get; set; } = new List<ResultadosInspeccion>();
}
