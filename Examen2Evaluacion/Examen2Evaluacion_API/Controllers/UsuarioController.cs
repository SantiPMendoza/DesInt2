using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Examen2Evaluacion_API.Models.DTOs;
using Examen2Evaluacion_API.Models.Entity;
using Examen2Evaluacion_API.Repository.IRepository;

namespace Examen2Evaluacion_API.Controllers
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
