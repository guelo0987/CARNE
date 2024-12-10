using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class Sancione
{
    public int IdSancion { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Monto { get; set; }

    public virtual ICollection<SancionIrregularidad> SancionIrregularidads { get; set; } = new List<SancionIrregularidad>();
}
