using AutoMapper;
using RestAPI.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestAPI.Controllers;
using RestAPI.Models.DTOs.AlumnoDTO;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : BaseController<Alumno, AlumnoDto, CreateAlumnoDTO>
    {
        public AlumnoController(IAlumnoRepository alumnoRepository, IMapper mapper, ILogger<AlumnoController> logger)
            : base(alumnoRepository, mapper, logger)
        {
        }
    }
}
