namespace CARNE.DTO;

public class NormativaDTO
{
    public int IdNormativa { get; set; }

    public string NombreNormativa { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Version { get; set; }

    public DateTime? FechaAdmision { get; set; }

    public DateTime? FechaVigencia { get; set; }
}