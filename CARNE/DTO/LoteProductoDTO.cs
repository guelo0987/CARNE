namespace CARNE.DTO;

public class LoteProductoDTO
{
    public int IdLote { get; set; }

    public int IdEstablecimiento { get; set; }

    public string CodigoLote { get; set; } = null!;

    public DateTime? FechaProduccion { get; set; }

    public string? DescripcionProducto { get; set; }

    public string? DestinoFinal { get; set; }
}