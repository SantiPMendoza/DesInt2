using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepasoAPI.Controllers;
using RepasoAPI.Models.DTOs;
using RepasoAPI.Models.Entity;
using RepasoAPI.Repository.IRepository;


namespace RepasoAPI.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : BaseController<Producto, ProductoDTO, CreateProductoDTO>
    {
        public ProductoController(IProductoRepository productoRepository, IMapper mapper, ILogger<ProductoController> logger)
            : base(productoRepository, mapper, logger)
        {
        }
    }
}
