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
    public class FranjaHorariaController : BaseController<FranjaHoraria, FranjaHorariaDTO, CreateFranjaHorariaDTO>
    {
        public FranjaHorariaController(IFranjaHorariaRepository franjaHorariaRepository, IMapper mapper, ILogger<FranjaHorariaController> logger)
            : base(franjaHorariaRepository, mapper, logger)
        {
        }
    }
}
