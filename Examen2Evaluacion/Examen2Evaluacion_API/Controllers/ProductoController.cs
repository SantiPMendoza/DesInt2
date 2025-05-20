using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Examen2Evaluacion_API.Controllers;
using Examen2Evaluacion_API.Models.DTOs;
using Examen2Evaluacion_API.Models.Entity;
using Examen2Evaluacion_API.Repository.IRepository;


namespace Examen2Evaluacion_API.Controllers
{
    [AllowAnonymous] // Cambiar a [Authorize(Roles = "admin")] si se requiere autenticación
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
