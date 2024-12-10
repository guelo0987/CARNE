using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class Animale
{
    public int IdAnimal { get; set; }

    public int IdEstablecimientoSacrificio { get; set; }

    public string IdentificacionAnimal { get; set; } = null!;

    public string Especie { get; set; } = null!;

    public DateTime? FechaSacrificio { get; set; }

    public virtual Establecimiento IdEstablecimientoSacrificioNavigation { get; set; } = null!;

    public virtual ICollection<LotesProducto> IdLotes { get; set; } = new List<LotesProducto>();
}
