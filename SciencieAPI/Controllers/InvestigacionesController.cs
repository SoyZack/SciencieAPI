using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SciencieAPI.Data;
using SciencieAPI.Dtos.Investigaciones;
using SciencieAPI.Models;

namespace SciencieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvestigacionesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public InvestigacionesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/investigaciones (Lista todas las investigaciones)[cite: 2]
        [HttpGet]
        public async Task<IActionResult> GetInvestigaciones()
        {
            var investigaciones = await _context.Investigacion.ToListAsync();
            var investigacionesDto = _mapper.Map<List<InvestigacionDto>>(investigaciones);
            return Ok(investigacionesDto);
        }

        // GET: api/investigaciones/estado/{estado} (Filtra investigaciones según su estado)[cite: 2]
        [HttpGet("estado/{estado}")]
        public async Task<IActionResult> GetInvestigacionesPorEstado(string estado)
        {
            var investigaciones = await _context.Investigacion
                .Where(i => i.Estado.ToLower() == estado.ToLower())
                .ToListAsync();

            var investigacionesDto = _mapper.Map<List<InvestigacionDto>>(investigaciones);
            return Ok(investigacionesDto);
        }

        // POST: api/investigaciones (Registra una nueva investigación)[cite: 2]
        [HttpPost]
        public async Task<IActionResult> CrearInvestigacion([FromBody] CrearInvestigacionDto crearDto)
        {
            var existeCientifico = await _context.Cientificos.AnyAsync(c => c.Id == crearDto.CientificoId);
            if (!existeCientifico)
                return BadRequest(new { mensaje = "El CientificoId especificado no existe." });

            var investigacion = _mapper.Map<Investigacion>(crearDto);
            _context.Investigacion.Add(investigacion);
            await _context.SaveChangesAsync();

            var investigacionDto = _mapper.Map<InvestigacionDto>(investigacion);
            return CreatedAtAction(nameof(GetInvestigaciones), new { id = investigacion.Id }, investigacionDto);
        }

        // PUT: api/investigaciones/{id} (Actualiza datos o estado de una investigación)[cite: 2]
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarInvestigacion(int id, [FromBody] ActualizarInvestigacionDto actualizarDto)
        {
            var investigacion = await _context.Investigacion.FindAsync(id);
            if (investigacion == null)
                return NotFound(new { mensaje = "Investigación no encontrada." });

            _mapper.Map(actualizarDto, investigacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/investigaciones/{id} (Elimina una investigación)[cite: 2]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarInvestigacion(int id)
        {
            var investigacion = await _context.Investigacion.FindAsync(id);
            if (investigacion == null)
                return NotFound(new { mensaje = "Investigación no encontrada." });

            _context.Investigacion.Remove(investigacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}