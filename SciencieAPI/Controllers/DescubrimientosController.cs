using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SciencieAPI.Dtos.Descubrimientos;
using SciencieAPI.Models;

namespace SciencieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DescubrimientosController : ControllerBase
    {
        private readonly DescubrimientosContext _context;
        private readonly IMapper _mapper;

        public DescubrimientosController(DescubrimientosContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Lista todos los descubrimientos
        [HttpGet]
        public async Task<IActionResult> GetDescubrimientos()
        {
            var descubrimientos = await _context.Descubrimientos.ToListAsync();
            var descubrimientosDto = _mapper.Map<List<DescubrimientosDto>>(descubrimientos);
            return Ok(descubrimientosDto);
        }

        // GET: Lista descubrimientos de un científico específico
        [HttpGet("/api/cientificos/{cientificoId}/descubrimientos")]
        public async Task<IActionResult> GetDescubrimientosPorCientifico(int cientificoId)
        {
            var existeCientifico = await _context.Cientificos.AnyAsync(c => c.Id == cientificoId);
            if (!existeCientifico)
                return NotFound(new { mensaje = "Científico no encontrado." });

            var descubrimientos = await _context.Descubrimientos
                .Where(d => d.CientificoId == cientificoId)
                .ToListAsync();

            var descubrimientosDto = _mapper.Map<List<DescubrimientosDto>>(descubrimientos);
            return Ok(descubrimientosDto);
        }

        // POST: Registra un nuevo descubrimiento
        [HttpPost]
        public async Task<IActionResult> CrearDescubrimiento([FromBody] CrearDescubrimientoDto crearDto)
        {
            var existeCientifico = await _context.Cientificos.AnyAsync(c => c.Id == crearDto.CientificoId);
            if (!existeCientifico)
                return BadRequest(new { mensaje = "El CientificoId especificado no existe." });

            var descubrimiento = _mapper.Map<Descubrimiento>(crearDto);
            _context.Descubrimientos.Add(descubrimiento);
            await _context.SaveChangesAsync();

            var descubrimientoDto = _mapper.Map<DescubrimientosDto>(descubrimiento);
            return CreatedAtAction(nameof(GetDescubrimientos), new { id = descubrimiento.Id }, descubrimientoDto);
        }

        // PUT: Actualiza un descubrimiento
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarDescubrimiento(int id, [FromBody] ActualizarDescubrimientoDto actualizarDto)
        {
            var descubrimiento = await _context.Descubrimientos.FindAsync(id);
            if (descubrimiento == null)
                return NotFound(new { mensaje = "Descubrimiento no encontrado." });

            _mapper.Map(actualizarDto, descubrimiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: Elimina un descubrimiento
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarDescubrimiento(int id)
        {
            var descubrimiento = await _context.Descubrimientos.FindAsync(id);
            if (descubrimiento == null)
                return NotFound(new { mensaje = "Descubrimiento no encontrado." });

            _context.Descubrimientos.Remove(descubrimiento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}