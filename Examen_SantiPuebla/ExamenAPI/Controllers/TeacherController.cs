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
    public class TeacherController : BaseController<Teacher, TeacherDTO, CreateTeacherDTO>
    {
        public TeacherController(ITeacherRepository repo, IMapper mapper, ILogger<TeacherController> logger)
            : base(repo, mapper, logger)
        {
        }
    }
}
