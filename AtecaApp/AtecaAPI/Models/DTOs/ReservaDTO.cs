
using System.ComponentModel.DataAnnotations;


namespace AtecaAPI.Models.DTOs{

public class ReservaDTO : CreateReservaDTO
{
    public int Id { get; set; }

    public ProfesorDTO Profesor { get; set; } = null!;
    public GrupoClaseDTO GrupoClase { get; set; } = null!;
    public FranjaHorariaDTO FranjaHoraria { get; set; } = null!;
    public string Estado { get; set; } = "Pendiente";
    public DateTime FechaSolicitud { get; set; }
    public DateTime? FechaResolucion { get; set; }
}

public class CreateReservaDTO
{
    [Required]
    public DateOnly Fecha { get; set; }

    [Required]
    public int FranjaHorariaId { get; set; }

    [Required]
    public int ProfesorId { get; set; }

    [Required]
    public int GrupoClaseId { get; set; }
}

}