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
    public class StudentController : BaseController<Student, StudentDTO, CreateStudentDTO>
    {
        public StudentController(IStudentRepository repo, IMapper mapper, ILogger<StudentController> logger)
            : base(repo, mapper, logger)
        {
        }
    }
}
