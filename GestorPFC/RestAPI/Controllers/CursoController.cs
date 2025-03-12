using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RestAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestAPI.Controllers;
using RestAPI.Models.DTOs.CursoDTO;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : BaseController<Curso, CursoDto, CreateCursoDTO>
    {
        public CursoController(ICursoRepository cursoRepository, IMapper mapper, ILogger<CursoController> logger)
            : base(cursoRepository, mapper, logger)
        {
        }
    }
}
