namespace CARNE.DTO;

public class SancionIrregularidadDTO
{
    public int IdIrregularidad { get; set; }

    public int IdSancion { get; set; }

    public DateTime? FechaAplicada { get; set; }

    public DateTime? FechaResolution { get; set; }

    public string? EstadoSancion { get; set; }

}