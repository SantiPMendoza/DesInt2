using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.ProfesorDTO
{
    public class CreateProfesorDTO
    {
        [Required(ErrorMessage = "Field required: Nombre")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Field required: Apellido")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "Field required: Correo")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Field required: DepartamentoId")]
        public int DepartamentoId { get; set; }
    }
}
