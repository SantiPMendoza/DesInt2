using System.ComponentModel.DataAnnotations;

namespace RestAPI.Models.DTOs.DepartamentoDTO
{
    public class CreateDepartamentoDTO
    {
        [Required(ErrorMessage = "Field required: Nombre")]
        public string Nombre { get; set; } = null!;
    }
}
