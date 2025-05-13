using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Examen2Evaluacion_API.Data;
using Examen2Evaluacion_API.Models.DTOs.UserDTO;
using Examen2Evaluacion_API.Models.Entity;
using Examen2Evaluacion_API.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Examen2Evaluacion_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly int TokenExpirationDays = 7;

        public UserRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            secretKey = config.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public AppUser GetUser(string id)
        {
            return _context.AppUsers.FirstOrDefault(user => user.Id == id);
        }

        public async Task<List<UserDTO>> GetUserDTOsAsync()
        {
            var users = await _context.Users.ToListAsync();
            var userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDTOs.Add(new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? ""
                });
            }
            return userDTOs;
        }

        public ICollection<AppUser> GetUsers()
        {
            return _context.AppUsers.OrderBy(user => user.UserName).ToList();
        }

        public bool IsUniqueUser(string userName)
        {
            return !_context.AppUsers.Any(user => user.UserName == userName);
        }

        public async Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDto)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.UserName.ToLower() == userLoginDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

            //user doesn't exist ?
            if (user == null || !isValid)
            {
                return new UserLoginResponseDTO { Token = "", User = null };
            }

            //User does exist
            var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();

            if (secretKey.Length < 32)
            {
                throw new ArgumentException("The secret key must be at least 32 characters long.");
            }
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())

                }),
                Expires = DateTime.UtcNow.AddMinutes(TokenExpirationDays),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            UserLoginResponseDTO userLoginResponseDto = new UserLoginResponseDTO
            {
                Token = tokenHandler.WriteToken(jwtToken),
                User = user
            };
            return userLoginResponseDto;
        }


        public async Task<UserLoginResponseDTO?> Register(UserRegistrationDTO userRegistrationDto)
        {
            AppUser user = new AppUser()
            {
                UserName = userRegistrationDto.UserName,
                Name = userRegistrationDto.Name,
                Email = userRegistrationDto.UserName,
                NormalizedEmail = userRegistrationDto.UserName.ToUpper(),
            };

            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);
            if (!result.Succeeded)
            {
                return null;
            }
            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole("admin"));
            if (!await _roleManager.RoleExistsAsync("wpf_user"))
                await _roleManager.CreateAsync(new IdentityRole("wpf_user"));
            if (!await _roleManager.RoleExistsAsync("angular_user"))
                await _roleManager.CreateAsync(new IdentityRole("angular_user"));

            await _userManager.AddToRoleAsync(user, "admin");
            AppUser? newUser = _context.AppUsers.FirstOrDefault(u => u.UserName == userRegistrationDto.UserName);

            return new UserLoginResponseDTO
            {
                User = newUser,
                Token = string.Empty
            };
        }

    }
}
