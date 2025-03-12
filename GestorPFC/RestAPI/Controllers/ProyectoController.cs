using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestAPI.Data;
using RestAPI.Models.DTOs.ProyectoDTO;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "alumno, profesor, admin")]
    public class ProyectoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProyectoRepository _proyectoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProyectoController> _logger;

        public ProyectoController(
            IProyectoRepository proyectoRepository,
            IMapper mapper,
            ILogger<ProyectoController> logger,
            ApplicationDbContext context)
        {
            _proyectoRepository = proyectoRepository;
            _mapper = mapper;
            _logger = logger;
            _context = context;
        }

        // GET: api/Proyecto
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (User.IsInRole("admin"))
            {
                var proyectos = await _proyectoRepository.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ProyectoDto>>(proyectos);
                return Ok(dtos);
            }
            else if (User.IsInRole("alumno"))
            {
                var alumnoClaim = User.FindFirst("AlumnoId")?.Value;
                if (!int.TryParse(alumnoClaim, out int alumnoId))
                    return Unauthorized("No se encontró el AlumnoId en las claims.");

                var proyectos = await _proyectoRepository.GetAllAsync();
                var proyectosFiltrados = proyectos.Where(p => p.AlumnoId == alumnoId);
                var dtos = _mapper.Map<IEnumerable<ProyectoDto>>(proyectosFiltrados);
                return Ok(dtos);
            }
            else if (User.IsInRole("profesor"))
            {
                var deptClaim = User.FindFirst("DepartamentoId")?.Value;
                if (!int.TryParse(deptClaim, out int deptId))
                    return Unauthorized("No se encontró el DepartamentoId en las claims.");

                var proyectos = await _proyectoRepository.GetAllAsync();
                var proyectosFiltrados = proyectos.Where(p => p.DepartamentoId == deptId);
                var dtos = _mapper.Map<IEnumerable<ProyectoDto>>(proyectosFiltrados);
                return Ok(dtos);
            }
            else
            {
                return Forbid();
            }
        }

        // POST: api/Proyecto
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProyectoDTO createDto)
        {
            if (User.IsInRole("admin"))
            {
                var proyecto = _mapper.Map<Proyecto>(createDto);
                proyecto.FechaEntrega = System.DateTime.UtcNow.AddDays(30);
                proyecto.EstadoProyecto = EstadoProyecto.Desarrollo;
                _context.Proyectos.Add(proyecto);
                await _context.SaveChangesAsync();
                var proyectoCreadoDto = _mapper.Map<ProyectoDto>(proyecto);
                return CreatedAtAction(nameof(GetAll), new { id = proyecto.Id }, proyectoCreadoDto);
            }
            else if (User.IsInRole("alumno"))
            {
                var alumnoClaim = User.FindFirst("AlumnoId")?.Value;
                if (!int.TryParse(alumnoClaim, out int alumnoId))
                    return Unauthorized("No se encontró el AlumnoId en las claims.");

                var proyecto = _mapper.Map<Proyecto>(createDto);
                proyecto.AlumnoId = alumnoId;
                proyecto.FechaEntrega = System.DateTime.UtcNow.AddDays(30);
                proyecto.EstadoProyecto = EstadoProyecto.Desarrollo;
                _context.Proyectos.Add(proyecto);
                await _context.SaveChangesAsync();
                var proyectoCreadoDto = _mapper.Map<ProyectoDto>(proyecto);
                return CreatedAtAction(nameof(GetAll), new { id = proyecto.Id }, proyectoCreadoDto);
            }
            else if (User.IsInRole("profesor"))
            {
                var deptClaim = User.FindFirst("DepartamentoId")?.Value;
                if (!int.TryParse(deptClaim, out int deptId))
                    return Unauthorized("No se encontró el DepartamentoId en las claims.");

                var dept = await _context.Departamentos.FirstOrDefaultAsync(d => d.Id == deptId);
                if (dept == null)
                    return NotFound("No se encontró el departamento del profesor.");

                var proyecto = _mapper.Map<Proyecto>(createDto);
                proyecto.DepartamentoId = dept.Id;
                proyecto.Departamento = dept;
                proyecto.FechaEntrega = System.DateTime.UtcNow.AddDays(30);
                proyecto.EstadoProyecto = EstadoProyecto.Desarrollo;
                _context.Proyectos.Add(proyecto);
                await _context.SaveChangesAsync();
                var proyectoCreadoDto = _mapper.Map<ProyectoDto>(proyecto);
                return CreatedAtAction(nameof(GetAll), new { id = proyecto.Id }, proyectoCreadoDto);
            }
            else
            {
                return Forbid();
            }
        }

        // PUT: api/Proyecto/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProyectoDto proyectoDto)
        {
            if (User.IsInRole("admin"))
            {
                var proyecto = await _context.Proyectos.FirstOrDefaultAsync(p => p.Id == id);
                if (proyecto == null)
                    return NotFound("Proyecto no encontrado.");

                _mapper.Map(proyectoDto, proyecto);
                _context.Proyectos.Update(proyecto);
                await _context.SaveChangesAsync();
                var updatedDto = _mapper.Map<ProyectoDto>(proyecto);
                return Ok(updatedDto);
            }
            else if (User.IsInRole("alumno"))
            {
                var alumnoClaim = User.FindFirst("AlumnoId")?.Value;
                if (!int.TryParse(alumnoClaim, out int alumnoId))
                    return Unauthorized("No se encontró el AlumnoId en las claims.");

                var proyecto = await _context.Proyectos.FirstOrDefaultAsync(p => p.Id == id && p.AlumnoId == alumnoId);
                if (proyecto == null)
                    return NotFound("Proyecto no encontrado o no pertenece al alumno.");

                _mapper.Map(proyectoDto, proyecto);
                _context.Proyectos.Update(proyecto);
                await _context.SaveChangesAsync();
                var updatedDto = _mapper.Map<ProyectoDto>(proyecto);
                return Ok(updatedDto);
            }
            else if (User.IsInRole("profesor"))
            {
                var deptClaim = User.FindFirst("DepartamentoId")?.Value;
                if (!int.TryParse(deptClaim, out int deptId))
                    return Unauthorized("No se encontró el DepartamentoId en las claims.");

                var proyecto = await _context.Proyectos.FirstOrDefaultAsync(p => p.Id == id && p.DepartamentoId == deptId);
                if (proyecto == null)
                    return NotFound("Proyecto no encontrado o no pertenece al departamento del profesor.");

                _mapper.Map(proyectoDto, proyecto);
                _context.Proyectos.Update(proyecto);
                await _context.SaveChangesAsync();
                var updatedDto = _mapper.Map<ProyectoDto>(proyecto);
                return Ok(updatedDto);
            }
            else
            {
                return Forbid();
            }
        }
    }
}
