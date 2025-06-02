using System.ComponentModel.DataAnnotations;

public class CreateJuegoDTO
{
    [Required(ErrorMessage = "Field required: Nombre")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Field required: UserId")]
    public string AppUserId { get; set; }

    public DateTime Fecha_Inicio { get; set; }
    public DateTime Fecha_Fin { get; set; }

    public double Resultado { get; set; }
}
