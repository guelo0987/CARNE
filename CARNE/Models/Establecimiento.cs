using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class Establecimiento
{
    public int IdEstablecimiento { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Comerciales { get; set; }

    public string? TipoOperacion { get; set; }

    public string? TipoProducto { get; set; }

    public string? CapacidadOperativa { get; set; }

    public string? VolumenProcesado { get; set; }

    public string? UnidadVolumen { get; set; }

    public string? PeriodoVolumen { get; set; }

    public string? Riesgo { get; set; }

    public string? LicenciasCertificaciones { get; set; }

    public string? EstadoEstablecimiento { get; set; }

    public virtual ICollection<Animale> Animales { get; set; } = new List<Animale>();

    public virtual ICollection<Inspeccione> Inspecciones { get; set; } = new List<Inspeccione>();

    public virtual ICollection<Irregularidad> Irregularidads { get; set; } = new List<Irregularidad>();

    public virtual ICollection<LotesProducto> LotesProductos { get; set; } = new List<LotesProducto>();
}
