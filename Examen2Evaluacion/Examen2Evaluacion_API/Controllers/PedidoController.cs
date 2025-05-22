using AutoMapper;
using Examen2Evaluacion_API.Controllers;
using Examen2Evaluacion_API.Data;
using Examen2Evaluacion_API.Models.DTOs;
using Examen2Evaluacion_API.Models.Entity;
using Examen2Evaluacion_API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class PedidoController : BaseController<Pedido, PedidoDTO, CreatePedidoDTO>
{
    private readonly ApplicationDbContext _context;

    public PedidoController(IPedidoRepository pedidoRepository, ApplicationDbContext context, IMapper mapper, ILogger<PedidoController> logger)
        : base(pedidoRepository, mapper, logger)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public override async Task<IActionResult> Create([FromBody] CreatePedidoDTO createDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pedido = _mapper.Map<Pedido>(createDto);

            // Cargar los productos desde la base de datos
            var productos = await _context.Productos
                .Where(p => createDto.Productos.Contains(p.Id))
                .ToListAsync();

            pedido.Productos = productos;

            if (!await _repository.CreateAsync(pedido))
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error creating pedido");

            var dto = _mapper.Map<PedidoDTO>(pedido);
            return CreatedAtRoute("Pedido_GetEntity", new { id = dto.Id }, dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating pedido");
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.InnerException?.Message ?? ex.Message);
        }

    }
}
