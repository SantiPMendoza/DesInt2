using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.CursoDTO
{
    public class CreateCursoDTO
    {
        [Required(ErrorMessage = "Field required: Nombre")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Field required: DepartamentoId")]
        public int DepartamentoId { get; set; }

        [Required(ErrorMessage = "Field required: TutorId")]
        public int TutorId { get; set; }
    }
}
