namespace CARNE.DTO;

public class ListaDTO
{
    public int IdLista { get; set; }

    public int IdNormativa { get; set; }

    public string NombreLista { get; set; } = null!;

    public string? Descripcion { get; set; }
}