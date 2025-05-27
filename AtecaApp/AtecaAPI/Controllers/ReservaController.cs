using AtecaAPI.Models.DTOs;
using AtecaAPI.Models.DTOs.ReservaDTOs;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtecaAPI.Controllers
{
    [AllowAnonymous] // Cambiar cuando eso bro
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : BaseController<Reserva, ReservaDTO, CreateReservaDTO>
    {
        public ReservaController(IReservaRepository reservaRepository, IMapper mapper, ILogger<ReservaController> logger)
            : base(reservaRepository, mapper, logger)
        {
        }
    }
}
