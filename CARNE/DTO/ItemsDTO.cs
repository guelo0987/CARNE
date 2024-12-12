namespace CARNE.DTO;

public class ItemsDTO
{
    public int IdItem { get; set; }

    public int IdLista { get; set; }
    

    public string Descripcion { get; set; } = null!;

    public string? CriterioCumplimiento { get; set; }

}