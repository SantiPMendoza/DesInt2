using System.ComponentModel.DataAnnotations;

namespace Examen2Evaluacion_API.Models.DTOs
{
    public class UsuarioDTO : CreateUsuarioDTO
    {
        public int Id { get; set; }
        

    }

    public class CreateUsuarioDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
