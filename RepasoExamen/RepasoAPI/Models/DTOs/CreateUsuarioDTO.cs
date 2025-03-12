using System.ComponentModel.DataAnnotations;

namespace RepasoAPI.Models.DTOs
{
    public class CreateUsuarioDTO
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
