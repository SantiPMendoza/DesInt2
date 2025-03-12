using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepasoAPI.Models.DTOs;
using RepasoAPI.Models.Entity;
using RepasoAPI.Repository.IRepository;

namespace RepasoAPI.Controllers
{
    [Authorize(Roles = "admin")]
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
