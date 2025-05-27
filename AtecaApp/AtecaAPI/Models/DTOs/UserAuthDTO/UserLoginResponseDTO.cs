using AtecaAPI.Models.Entity;

namespace AtecaAPI.Models.DTOs.UserDTO
{
    public class UserLoginResponseDTO
    {
        public AppUser User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
