using AtecaAPI.Models.DTOs;
using AtecaAPI.Models.Entity;

namespace AtecaAPI.Models.DTOs
{
    public class ProfesorDTO : CreateProfesorDTO
    {
        public int Id { get; set; }

    }

    public class CreateProfesorDTO
    {
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string GoogleId { get; set; } = null!;
    }

}
