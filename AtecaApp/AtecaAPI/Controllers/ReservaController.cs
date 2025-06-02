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

        [HttpGet("aprobadas")]
        public async Task<IActionResult> GetAprobadas()
        {
            var reservasAprobadas = await _reservaRepository.GetAprobadasAsync();
            var reservasDto = _mapper.Map<IEnumerable<ReservaDTO>>(reservasAprobadas);
            return Ok(reservasDto);
        }


        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> RechazarReserva(int id)
        {
            var result = await _reservaRepository.RechazarReservaAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] CreateReservaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos de reserva inválidos.");

            var reserva = _mapper.Map<Reserva>(dto);
            reserva.FechaSolicitud = DateTime.Now;
            reserva.Estado = "Pendiente";

            var error = await _reservaRepository.ValidarReservaAsync(reserva);
            if (error != null)
                return Conflict(new { mensaje = error });   // ⬅️ Devuelve error específico

            var result = await _reservaRepository.CreateAsync(reserva);
            if (!result)
                return StatusCode(500, "Error interno al guardar la reserva.");

            var dtoFinal = _mapper.Map<ReservaDTO>(reserva);

            return CreatedAtRoute($"{ControllerContext.ActionDescriptor.ControllerName}_GetEntity", new { id = dtoFinal.Id }, dtoFinal);
        }


    }
}
