using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SciencieAPI.Dtos.Investigaciones;
using SciencieAPI.Models;

namespace SciencieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestigacionesController : ControllerBase
    {
        private readonly DescubrimientosContext _context;
        private readonly IMapper _mapper;

        public InvestigacionesController(DescubrimientosContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/investigaciones
        [HttpGet]
        public async Task<IActionResult> GetInvestigaciones()
        {
            var investigaciones = await _context.Investigaciones.ToListAsync();
            var investigacionesDto = _mapper.Map<List<InvestigacionDto>>(investigaciones);
            return Ok(investigacionesDto);
        }

        // GET: api/investigaciones/estado
        [HttpGet("estado/{estado}")]
        public async Task<IActionResult> GetInvestigacionesPorEstado(string estado)
        {
            var investigaciones = await _context.Investigaciones
                .Where(i => i.Estado.ToLower() == estado.ToLower())
                .ToListAsync();

            var investigacionesDto = _mapper.Map<List<InvestigacionDto>>(investigaciones);
            return Ok(investigacionesDto);
        }

        // POST: api/investigaciones
        [HttpPost]
        public async Task<IActionResult> CrearInvestigacion([FromBody] CrearInvestigacionDto crearDto)
        {
            var existeCientifico = await _context.Cientificos.AnyAsync(c => c.Id == crearDto.CientificoId);
            if (!existeCientifico)
                return BadRequest(new { mensaje = "El CientificoId especificado no existe." });

            var investigacion = _mapper.Map<Investigacion>(crearDto);
            _context.Investigaciones.Add(investigacion);
            await _context.SaveChangesAsync();

            var investigacionDto = _mapper.Map<InvestigacionDto>(investigacion);
            return CreatedAtAction(nameof(GetInvestigaciones), new { id = investigacion.Id }, investigacionDto);
        }

        // PUT: api/investigaciones
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarInvestigacion(int id, [FromBody] ActualizarInvestigacionDto actualizarDto)
        {
            var investigacion = await _context.Investigaciones.FindAsync(id);
            if (investigacion == null)
                return NotFound(new { mensaje = "Investigación no encontrada." });

            _mapper.Map(actualizarDto, investigacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/investigaciones
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarInvestigacion(int id)
        {
            var investigacion = await _context.Investigaciones.FindAsync(id);
            if (investigacion == null)
                return NotFound(new { mensaje = "Investigación no encontrada." });

            _context.Investigaciones.Remove(investigacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}