using RestAPI.Models.Entity;

namespace RestAPI.Models.DTOs.UserDTO
{
    public class UserLoginResponseDTO
    {
        public AppUser User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
