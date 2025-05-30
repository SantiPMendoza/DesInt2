using AtecaAPI.Controllers;
using AtecaAPI.Models.DTOs;
using AtecaAPI.Models.Entity;
using AtecaAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtecaAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Cambiar cuando eso mi pana

    public class ReservaController : BaseController<Reserva, ReservaDTO, CreateReservaDTO>
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservaController(IReservaRepository reservaRepository, IMapper mapper, ILogger<ReservaController> logger)
            : base(reservaRepository, mapper, logger)
        {
            _reservaRepository = reservaRepository;
        }

        [HttpPut("{id}/aceptar")]
        public async Task<IActionResult> AceptarReserva(int id)
        {
            var result = await _reservaRepository.AceptarReservaAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> RechazarReserva(int id)
        {
            var result = await _reservaRepository.RechazarReservaAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
