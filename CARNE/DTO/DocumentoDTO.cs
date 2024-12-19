namespace CARNE.DTO;

public class DocumentoDTO
{
    public int IdDocumento { get; set; }

    public int IdLote { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public DateTime? FechaEmision { get; set; }

    public DateTime? FechaVencimiento { get; set; }
}