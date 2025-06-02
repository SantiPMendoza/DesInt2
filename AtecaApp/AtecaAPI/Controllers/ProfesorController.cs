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
        private readonly IProfesorRepository _profesorRepository;
        public ProfesorController(IProfesorRepository profesorRepository, IMapper mapper, ILogger<ProfesorController> logger)
            : base(profesorRepository, mapper, logger)
        {
            _profesorRepository = profesorRepository;
        }

        [HttpGet("google/{googleId}")]
        public async Task<ActionResult<ProfesorDTO>> GetByGoogleId(string googleId)
        {
            var profesor = await _profesorRepository.GetByGoogleIdAsync(googleId);
            if (profesor == null)
                return NotFound();

            return Ok(_mapper.Map<ProfesorDTO>(profesor));
        }


        [AllowAnonymous]
        [HttpPost("create-if-not-exists")]
        public async Task<IActionResult> CreateIfNotExists([FromBody] CreateProfesorDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.GoogleId) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest("Datos incompletos.");

            var existingProfesor = await ((IProfesorRepository)_repository).GetByGoogleIdAsync(dto.GoogleId);

            if (existingProfesor != null)
            {
                var profesorDto = _mapper.Map<ProfesorDTO>(existingProfesor);
                return Ok(profesorDto);
            }

            var nuevoProfesor = _mapper.Map<Profesor>(dto);

            var created = await ((IProfesorRepository)_repository).CreateIfNotExistsAsync(nuevoProfesor);

            if (!created)
                return Conflict("El profesor ya existe.");

            var profesorCreado = await ((IProfesorRepository)_repository).GetByGoogleIdAsync(nuevoProfesor.GoogleId);
            var profesorCreadoDto = _mapper.Map<ProfesorDTO>(profesorCreado);

            return CreatedAtRoute($"{ControllerContext.ActionDescriptor.ControllerName}_GetEntity", new { id = profesorCreadoDto.Id }, profesorCreadoDto);
        }



    }
}
