using System.ComponentModel.DataAnnotations;

namespace Examen2Evaluacion_API.Models.DTOs.UserDTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Field required: UserName")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Field required: Password")]
        public string Password { get; set; } = null!;
    }
}
