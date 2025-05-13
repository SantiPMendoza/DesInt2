using Examen2Evaluacion_API.Models.Entity;

namespace Examen2Evaluacion_API.Models.DTOs.UserDTO
{
    public class UserLoginResponseDTO
    {
        public AppUser User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
