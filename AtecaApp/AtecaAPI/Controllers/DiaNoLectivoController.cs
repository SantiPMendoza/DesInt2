using AtecaAPI.Models.DTOs;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository;
using AtecaAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtecaAPI.Controllers
{
    [Authorize] // Cambiar cuando eso bro
    [Route("api/[controller]")]
    [ApiController]
    public class DiaNoLectivoController : BaseController<DiaNoLectivo, DiaNoLectivoDTO, CreateDiaNoLectivoDTO>
    {
        public DiaNoLectivoController(IDiaNoLectivoRepository diaNoLectivoRepository, IMapper mapper, ILogger<DiaNoLectivoController> logger)
            : base(diaNoLectivoRepository, mapper, logger)
        {
        }
    }
}
