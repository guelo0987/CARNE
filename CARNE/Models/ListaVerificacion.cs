using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class ListaVerificacion
{
    public int IdLista { get; set; }

    public int IdNormativa { get; set; }

    public string NombreLista { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual Normativa IdNormativaNavigation { get; set; } = null!;

    public virtual ICollection<ItemsVerificacion> ItemsVerificacions { get; set; } = new List<ItemsVerificacion>();

    public virtual ICollection<ResultadosInspeccion> ResultadosInspeccions { get; set; } = new List<ResultadosInspeccion>();
}
