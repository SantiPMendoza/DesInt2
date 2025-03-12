using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.PropuestaDTO
{
    public class CreatePropuestaDTO
    {
        [Required(ErrorMessage = "Field required: Titulo")]
        public string Titulo { get; set; } = null!;

        [Required(ErrorMessage = "Field required: Descripcion")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "Field required: Departamento")]
        public string Departamento { get; set; } = null!;

        [Required(ErrorMessage = "Field required: BooleanProyecto")]
        public bool BooleanProyecto { get; set; }

        [Required(ErrorMessage = "Field required: AlumnoId")]
        public int AlumnoId { get; set; }
    }
}
