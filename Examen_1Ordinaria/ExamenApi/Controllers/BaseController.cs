﻿using AutoMapper;
using ExamenApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamenApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TEntity, TDto, TCreateDto> : ControllerBase
        where TEntity : class
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;

        protected BaseController(IRepository<TEntity> repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        [Authorize(Roles = "admin,WPF_User")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var entities = _mapper.Map<List<TDto>>(await _repository.GetAllAsync());
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(Roles = "admin,WPF_User")]
        [HttpGet("{id:int}", Name = "[controller]_GetEntity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var entity = await _repository.GetAsync(id);
                if (entity == null) return NotFound();

                return Ok(_mapper.Map<TDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "admin,WPF_User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] TCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var entity = _mapper.Map<TEntity>(createDto);
                await _repository.CreateAsync(entity);

                var dto = _mapper.Map<TDto>(entity);


                var idProperty = dto.GetType().GetProperty("Id");
                var id = idProperty?.GetValue(dto);

                return CreatedAtRoute($"{ControllerContext.ActionDescriptor.ControllerName}_GetEntity", new { id }, dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "admin,WPF_User")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] TDto dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var entity = await _repository.GetAsync(id);
                if (entity == null) return NotFound();

                _mapper.Map(dto, entity);
                await _repository.UpdateAsync(entity);

                return Ok(_mapper.Map<TDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(Roles = "admin,WPF_User")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _repository.GetAsync(id);
                if (entity == null) return NotFound();

                await _repository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
