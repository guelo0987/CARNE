namespace CARNE.DTO;

public class ResultadoDTO
{
    public int IdResultado { get; set; }

    public int IdInspeccion { get; set; }

    public int IdLista { get; set; }

    public int IdItem { get; set; }

    public bool Cumple { get; set; }

    public string? Observacion { get; set; }
}