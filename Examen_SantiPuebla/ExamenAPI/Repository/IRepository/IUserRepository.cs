using ExamenAPI.Models.DTOs.UserDTO;
using ExamenAPI.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExamenAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        ICollection<AppUser> GetUsers();
        AppUser GetUser(string id);
        bool IsUniqueUser(string userName);
        Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDto);
        Task<UserLoginResponseDTO?> Register(UserRegistrationDTO userRegistrationDto);

        Task<List<UserDTO>> GetUserDTOsAsync();
    }
}
