using System.ComponentModel.DataAnnotations;

namespace AtecaAPI.Models.DTOs
{
    public class AdministradorDTO : CreateAdministradorDTO
    {
        public int Id { get; set; }
    }

    public class CreateAdministradorDTO
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
