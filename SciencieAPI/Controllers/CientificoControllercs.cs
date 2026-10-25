using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SciencieAPI.Dtos.Cientificos;
using SciencieAPI.Models;

namespace SciencieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CientificosController : ControllerBase
    {
        private readonly DescubrimientosContext _context;
        private readonly IMapper _mapper;

        public CientificosController(DescubrimientosContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 1. GET: api/cientificos
        [HttpGet]
        public async Task<IActionResult> GetCientificos()
        {
            var cientificos = await _context.Cientificos
                .Include(c => c.Especialidades) 
                .ToListAsync();

            var cientificosDto = _mapper.Map<List<CientificoDto>>(cientificos);
            return Ok(cientificosDto);
        }

        // 2. GET: api/cientificos
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCientificoPorId(int id)
        {
            var cientifico = await _context.Cientificos
                .Include(c => c.Especialidades)
                .Include(c => c.Descubrimientos)
                .Include(c => c.Investigaciones)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cientifico == null)
                return NotFound(new { mensaje = "Científico no encontrado." });

            var cientificoDetalleDto = _mapper.Map<CientificoDetalleDto>(cientifico);
            return Ok(cientificoDetalleDto);
        }

        // 3. POST: api/cientificos
        [HttpPost]
        public async Task<IActionResult> CrearCientifico([FromBody] CrearCientificoDto crearDto)
        {
            var idsEspecialidades = crearDto.EspecialidadIds?.Distinct().ToList() ?? new List<int>();
            var cientifico = _mapper.Map<Cientifico>(crearDto);

            if (idsEspecialidades.Count > 0)
            {
                var especialidadesDb = await _context.Especialidades
                    .Where(e => idsEspecialidades.Contains(e.Id))
                    .ToListAsync();

                var faltantes = idsEspecialidades.Except(especialidadesDb.Select(e => e.Id)).ToList();
                if (faltantes.Count > 0)
                    return BadRequest(new { mensaje = $"No existen las especialidades: {string.Join(", ", faltantes)}." });

                // Asignación directa a la colección
                foreach (var esp in especialidadesDb)
                {
                    cientifico.Especialidades.Add(esp);
                }
            }

            _context.Cientificos.Add(cientifico);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCientificoPorId), new { id = cientifico.Id }, new { mensaje = "Científico creado exitosamente.", id = cientifico.Id });
        }

        // 4. PUT: api/cientificos
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCientifico(int id, [FromBody] ActualizarCientificoDto actualizarDto)
        {
            var cientifico = await _context.Cientificos.FindAsync(id);
            if (cientifico == null)
                return NotFound(new { mensaje = "Científico no encontrado." });

            _mapper.Map(actualizarDto, cientifico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. DELETE: api/cientificos
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCientifico(int id)
        {
            var cientifico = await _context.Cientificos.FindAsync(id);
            if (cientifico == null)
                return NotFound(new { mensaje = "Científico no encontrado." });

            // Al aplicar ON DELETE CASCADE en SQL Server, EF Core y la base de datos 
            // se encargan automáticamente de borrar descubrimientos e investigaciones asociados.
            _context.Cientificos.Remove(cientifico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 6. POST: api/cientificos/{id}/especialidades
        [HttpPost("{id}/especialidades/{especialidadId}")]
        public async Task<IActionResult> AsignarEspecialidad(int id, int especialidadId)
        {
            // Incluimos las especialidades para poder validarlas y manipularlas
            var cientifico = await _context.Cientificos
                .Include(c => c.Especialidades)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cientifico == null) return NotFound(new { mensaje = "Científico no encontrado." });

            var especialidad = await _context.Especialidades.FindAsync(especialidadId);
            if (especialidad == null) return NotFound(new { mensaje = "Especialidad no encontrada." });

            if (cientifico.Especialidades.Any(e => e.Id == especialidadId))
                return BadRequest(new { mensaje = "El científico ya tiene asignada esta especialidad." });

            cientifico.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Especialidad asignada correctamente al científico." });
        }

        // 7. DELETE: api/cientificos/{id}/especialidades
        [HttpDelete("{id}/especialidades/{especialidadId}")]
        public async Task<IActionResult> RemoverEspecialidad(int id, int especialidadId)
        {
            var cientifico = await _context.Cientificos
                .Include(c => c.Especialidades)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cientifico == null) return NotFound(new { mensaje = "Científico no encontrado." });

            var especialidad = cientifico.Especialidades.FirstOrDefault(e => e.Id == especialidadId);
            if (especialidad == null)
                return NotFound(new { mensaje = "La relación entre el científico y la especialidad no existe." });

            cientifico.Especialidades.Remove(especialidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}