using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestAPI.Controllers;
using RestAPI.Models.DTOs.ProfesorDTO;
using RestAPI.Models.Entity;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : BaseController<Profesor, ProfesorDto, CreateProfesorDTO>
    {
        public ProfesorController(IProfesorRepository profesorRepository, IMapper mapper, ILogger<ProfesorController> logger)
            : base(profesorRepository, mapper, logger)
        {
        }
    }
}
