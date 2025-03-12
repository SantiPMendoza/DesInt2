using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepasoAPI.Models.DTOs;
using RepasoAPI.Models.Entity;
using RepasoAPI.Repository.IRepository;

namespace RepasoAPI.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController<Usuario, UsuarioDTO, CreateUsuarioDTO>
    {
        public UsuarioController(IUsuarioRepository usuarioRepository, IMapper mapper, ILogger<UsuarioController> logger)
            : base(usuarioRepository, mapper, logger)
        {
        }
    }
}
