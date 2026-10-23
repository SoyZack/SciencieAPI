using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SciencieAPI.Data;
using SciencieAPI.Dtos.Descubrimientos;
using SciencieAPI.Models;

namespace SciencieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DescubrimientosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DescubrimientosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/descubrimientos (Lista todos los descubrimientos)[cite: 2]
        [HttpGet]
        public async Task<IActionResult> GetDescubrimientos()
        {
            var descubrimientos = await _context.Descubrimientos.ToListAsync();
            var descubrimientosDto = _mapper.Map<List<DescubrimientosDto>>(descubrimientos);
            return Ok(descubrimientosDto);
        }

        // GET: api/cientificos/{cientificoId}/descubrimientos (Lista descubrimientos de un científico específico)[cite: 2]
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

        // POST: api/descubrimientos (Registra un nuevo descubrimiento)[cite: 2]
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

        // PUT: api/descubrimientos/{id} (Actualiza un descubrimiento)[cite: 2]
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

        // DELETE: api/descubrimientos/{id} (Elimina un descubrimiento)[cite: 2]
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