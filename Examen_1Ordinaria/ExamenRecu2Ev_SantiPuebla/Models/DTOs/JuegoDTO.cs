using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class JuegoDTO : CreateJuegoDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }


}
public class CreateJuegoDTO
{

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("appUserId")]
    public string AppUserId { get; set; } = string.Empty;

    [JsonPropertyName("fecha_inicio")]
    public DateTime Fecha_Inicio { get; set; }

    [JsonPropertyName("fecha_fin")]
    public DateTime Fecha_Fin { get; set; }

    [JsonPropertyName("resultado")]
    public double Resultado { get; set; }
}

