using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.ProyectoDTO
{
    public class CreateProyectoDTO
    {
        [Required(ErrorMessage = "Field required: Titulo")]
        public string Titulo { get; set; } = null!;

        [Required(ErrorMessage = "Field required: Descripcion")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "Field required: FechaEntrega")]
        public DateTime FechaEntrega { get; set; }

        [Required(ErrorMessage = "Field required: DepartamentoId")]
        public int DepartamentoId { get; set; }

        [Required(ErrorMessage = "Field required: AlumnoId")]
        public int AlumnoId { get; set; }

        public int? TutorProyectoId { get; set; }
    }
}
