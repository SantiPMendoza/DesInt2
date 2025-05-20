using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Examen2Evaluacion_API.Models.DTOs;
using Examen2Evaluacion_API.Models.Entity;
using Examen2Evaluacion_API.Repository.IRepository;

namespace Examen2Evaluacion_API.Controllers
{
    [AllowAnonymous] // Cambiar a [Authorize(Roles = "admin")] si se requiere autenticación
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : BaseController<Pedido, PedidoDTO, CreatePedidoDTO>
    {
        public PedidoController(IPedidoRepository pedidoRepository, IMapper mapper, ILogger<PedidoController> logger)
            : base(pedidoRepository, mapper, logger)
        {
        }
    }
}
