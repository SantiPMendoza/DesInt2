using AutoMapper;
using RestAPI.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestAPI.Controllers;
using RestAPI.Models.DTOs.DepartamentoDTO;
using RestAPI.Repository.IRepository;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : BaseController<Departamento, DepartamentoDto, CreateDepartamentoDTO>
    {
        public DepartamentoController(IDepartamentoRepository departamentoRepository, IMapper mapper, ILogger<DepartamentoController> logger)
            : base(departamentoRepository, mapper, logger)
        {
        }
    }
}
