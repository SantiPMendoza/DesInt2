using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using RestAPI.Data;
using RestAPI.Models.DTOs.UserDTO;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RestAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly int TokenExpirationMinutes = 30;

        public UserRepository(ApplicationDbContext context, IConfiguration config,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            _secretKey = config.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public ICollection<AppUser> GetUsers()
        {
            return _context.Users.OrderBy(u => u.UserName).ToList();
        }

        public AppUser GetUser(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public bool IsUniqueUser(string userName)
        {
            return !_context.Users.Any(u => u.UserName == userName);
        }

        public async Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == userLoginDto.UserName.ToLower());
            if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                return new UserLoginResponseDTO { Token = "", User = null };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "")
            };

            if (roles.Contains("alumno"))
            {
                var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.Email.ToLower() == user.Email.ToLower());
                if (alumno != null)
                    claims.Add(new Claim("AlumnoId", alumno.Id.ToString()));
            }
            if (roles.Contains("profesor"))
            {
                var profesor = await _context.Profesores.FirstOrDefaultAsync(p => p.Email.ToLower() == user.Email.ToLower());
                if (profesor != null)
                    claims.Add(new Claim("DepartamentoId", profesor.DepartamentoId.ToString()));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.UtcNow.AddMinutes(TokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            return new UserLoginResponseDTO
            {
                Token = tokenHandler.WriteToken(jwtToken),
                User = user
            };
        }

        public async Task<UserLoginResponseDTO> Register(UserRegistrationDTO userRegistrationDto)
        {
            string roleToAssign = string.Empty;
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.Email.ToLower() == userRegistrationDto.Email.ToLower());
            if (alumno != null)
                roleToAssign = "alumno";
            else
            {
                var profesor = await _context.Profesores.FirstOrDefaultAsync(p => p.Email.ToLower() == userRegistrationDto.Email.ToLower());
                if (profesor != null)
                    roleToAssign = "profesor";
                else
                {
                    var curso = _context.Cursos.FirstOrDefault();
                    if (curso == null)
                        throw new Exception("No existe ningún curso para asociar al alumno.");
                    alumno = new Alumno
                    {
                        Nombre = userRegistrationDto.Name,
                        Apellidos = "SinDefinir",
                        Email = userRegistrationDto.Email,
                        CursoId = curso.Id,
                        Curso = curso
                    };
                    _context.Alumnos.Add(alumno);
                    await _context.SaveChangesAsync();
                    roleToAssign = "alumno";
                }
            }

            AppUser user = new AppUser()
            {
                UserName = userRegistrationDto.UserName,
                Name = userRegistrationDto.Name,
                Email = userRegistrationDto.Email,
                NormalizedEmail = userRegistrationDto.Email.ToUpper()
            };

            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al crear el usuario: {errors}");
            }

            if (!await _roleManager.RoleExistsAsync("admin"))
                await _roleManager.CreateAsync(new IdentityRole("admin"));
            if (!await _roleManager.RoleExistsAsync("alumno"))
                await _roleManager.CreateAsync(new IdentityRole("alumno"));
            if (!await _roleManager.RoleExistsAsync("profesor"))
                await _roleManager.CreateAsync(new IdentityRole("profesor"));

            await _userManager.AddToRoleAsync(user, roleToAssign);
            return new UserLoginResponseDTO
            {
                User = user,
                Token = string.Empty
            };
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
    }
}
