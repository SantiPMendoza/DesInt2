using AtecaAPI.Models.DTOs;

using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtecaAPI.Controllers
{
    [Authorize] // Cambiar cuando eso bro
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoClaseController : BaseController<GrupoClase, GrupoClaseDTO, CreateGrupoClaseDTO>
    {
        public GrupoClaseController(IGrupoClaseRepository grupoClaseRepository, IMapper mapper, ILogger<GrupoClaseController> logger)
            : base(grupoClaseRepository, mapper, logger)
        {
        }
    }
}
