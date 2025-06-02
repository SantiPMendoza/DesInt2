using AutoMapper;
using ExamenApi.Models;
using ExamenApi.Models.DTOs.JuegoDTO;
using ExamenApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamenApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class JuegoController : BaseController<Juego, JuegoDTO, CreateJuegoDTO>
    {
        public JuegoController(IJuegoRepository repository, IMapper mapper, ILogger<JuegoController> logger)
            : base(repository, mapper, logger)
        {
        }

        [HttpGet("top10")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTop10()
        {
            try
            {
                var topJuegos = await ((IJuegoRepository)_repository).GetTop10Async();
                var dtos = _mapper.Map<IEnumerable<JuegoDTO>>(topJuegos);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el ranking");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
