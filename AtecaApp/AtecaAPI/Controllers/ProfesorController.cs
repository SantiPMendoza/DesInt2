using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AtecaAPI.Models.DTOs;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;

namespace AtecaAPI.Controllers
{
    [AllowAnonymous] // Cambiar cuando eso bro
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : BaseController<Profesor, ProfesorDTO, CreateProfesorDTO>
    {
        public ProfesorController(IProfesorRepository usuarioRepository, IMapper mapper, ILogger<ProfesorController> logger)
            : base(usuarioRepository, mapper, logger)
        {
        }
    }
}
