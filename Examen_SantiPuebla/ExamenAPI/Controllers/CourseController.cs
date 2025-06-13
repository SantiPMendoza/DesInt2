using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExamenAPI.Models.DTOs;
using ExamenAPI.Models.Entity;
using ExamenAPI.Repository.IRepository;

namespace ExamenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Cambiar cuando toque seguridad
    public class CourseController : BaseController<Course, CourseDTO, CreateCourseDTO>
    {
        private ICourseRepository _repo;
        public CourseController(ICourseRepository repo, IMapper mapper, ILogger<CourseController> logger)
            : base(repo, mapper, logger)
        {
            _repo = repo;
        }

        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] CreateCourseDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos de course inválidos.");

            var course = _mapper.Map<Course>(dto);

            // Cargar y asignar relaciones N:N
            course.Students = await _repo.LoadStudentsByIds(dto.StudentIds);
            course.Teachers = await _repo.LoadTeachersByIds(dto.TeacherIds);

            var result = await _repo.CreateAsync(course);
            if (!result)
                return StatusCode(500, "Error interno al guardar el curso.");

            var dtoFinal = _mapper.Map<CourseDTO>(course);

            return CreatedAtRoute($"{ControllerContext.ActionDescriptor.ControllerName}_GetEntity", new { id = dtoFinal.Id }, dtoFinal);
        }

    }
}
