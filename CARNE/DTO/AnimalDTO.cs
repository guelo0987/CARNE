namespace CARNE.DTO;

public class AnimalDTO
{
    public int IdAnimal { get; set; }

    public int IdEstablecimientoSacrificio { get; set; }

    public string IdentificacionAnimal { get; set; } = null!;

    public string Especie { get; set; } = null!;

    public DateTime? FechaSacrificio { get; set; }
}