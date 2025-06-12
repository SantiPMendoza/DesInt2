using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExamenAPI.Models.DTOs;
using ExamenAPI.Models.Entity;
using ExamenAPI.Repository.IRepository;

namespace ExamenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]//[Authorize(Roles = "admin")] // Cambiar cuando eso mi pana
    public class AdministradorController : BaseController<Administrador, AdministradorDTO, CreateAdministradorDTO>
    {
        public AdministradorController(IAdministradorRepository repo, IMapper mapper, ILogger<AdministradorController> logger)
            : base(repo, mapper, logger)
        {
        }
    }
}
