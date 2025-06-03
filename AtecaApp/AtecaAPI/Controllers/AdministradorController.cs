using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AtecaAPI.Models.DTOs;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;

namespace AtecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")] // Cambiar cuando eso mi pana
    public class AdministradorController : BaseController<Administrador, AdministradorDTO, CreateAdministradorDTO>
    {
        public AdministradorController(IAdministradorRepository repo, IMapper mapper, ILogger<AdministradorController> logger)
            : base(repo, mapper, logger)
        {
        }
    }
}
