using Examen1Evaluacion_API.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Examen1Evaluacion_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanetController : Controller
    {
        private readonly ILogger<Planet> _logger;

        public PlanetController(ILogger<Planet> logger)
        {
            _logger = logger;
        }

        private static List<Planet> planets = new List<Planet>
        {
            new Planet { Id = 1, Name = "Earth", Distance = 149, Atmosphere = "Nitrogen, Oxygen", Temperature = 15, ImageName = "earth.png" },
            new Planet { Id = 2, Name = "Mars", Distance = 228, Atmosphere = "CO2", Temperature = -60, ImageName = "mars.png" }
        };

        // GET All
        [HttpGet]
        public ActionResult<IEnumerable<Planet>> GetPlanets()
        {
            return Ok(planets);
        }

        // POST
        [HttpPost]
        public ActionResult<Planet> AddPlanet([FromBody] Planet newPlanet)
        {
            if (newPlanet == null)
                return BadRequest();

            newPlanet.Id = planets.Any() ? planets.Max(p => p.Id) + 1 : 1;
            planets.Add(newPlanet);
            return CreatedAtAction(nameof(GetPlanets), new { id = newPlanet.Id }, newPlanet);
        }
    }
}
