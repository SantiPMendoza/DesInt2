using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.AlumnoDTO
{
    public class CreateAlumnoDTO
    {
        [Required(ErrorMessage = "Field required: Nombre")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Field required: Apellidos")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "Field required: Correo")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Field required: CursoId")]
        public int CursoId { get; set; }
    }
}
