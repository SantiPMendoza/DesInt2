using AtecaAPI.Models.DTOs;
using AtecaAPI.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace AtecaAPI.Models.DTOs
{
    public class GrupoClaseDTO : CreateGrupoClaseDTO
    {
        public int Id { get; set; }

    }

    public class CreateGrupoClaseDTO
    {
        [Required]
        public string Nombre { get; set; } = null!;
    }
}
