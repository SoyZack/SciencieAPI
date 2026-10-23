using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SciencieAPI.Data;
using SciencieAPI.Dtos.Cientificos;
using SciencieAPI.Models;

namespace SciencieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CientificosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CientificosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 1. GET: api/cientificos (Retorna el listado de todos los científicos con sus especialidades)
        [HttpGet]
        public async Task<IActionResult> GetCientificos()
        {
            var cientificos = await _context.Cientificos
                .Include(c => c.CientificoEspecialidades)
                    .ThenInclude(ce => ce.Especialidad)
                .ToListAsync();

            var cientificosDto = _mapper.Map<List<CientificoDto>>(cientificos);
            return Ok(cientificosDto);
        }

        // 2. GET: api/cientificos/{id} (Detalle completo: especialidades, descubrimientos e investigaciones)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCientificoPorId(int id)
        {
            var cientifico = await _context.Cientificos
                .Include(c => c.CientificoEspecialidades)
                    .ThenInclude(ce => ce.Especialidad)
                .Include(c => c.Descubrimientos)
                .Include(c => c.Investigaciones)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cientifico == null)
                return NotFound(new { mensaje = "Científico no encontrado." });

            var cientificoDetalleDto = _mapper.Map<CientificoDetalleDto>(cientifico);
            return Ok(cientificoDetalleDto);
        }

        // 3. POST: api/cientificos (Crea un científico y le asigna especialidades opcionales)
        [HttpPost]
        public async Task<IActionResult> CrearCientifico([FromBody] CrearCientificoDto crearDto)
        {
            // Distintas para evitar duplicados enviados por el cliente.
            var idsEspecialidades = crearDto.EspecialidadIds?.Distinct().ToList() ?? new List<int>();

            // Valida las especialidades antes de crear el científico para no dejar registros incompletos.
            if (idsEspecialidades.Count > 0)
            {
                var existentes = await _context.Especialidades
                    .Where(e => idsEspecialidades.Contains(e.Id))
                    .Select(e => e.Id)
                    .ToListAsync();

                var faltantes = idsEspecialidades.Except(existentes).ToList();
                if (faltantes.Count > 0)
                    return BadRequest(new { mensaje = $"No existen las especialidades: {string.Join(", ", faltantes)}." });
            }

            var cientifico = _mapper.Map<Cientifico>(crearDto);
            _context.Cientificos.Add(cientifico);
            await _context.SaveChangesAsync();

            // Asocia las especialidades en la tabla intermedia CientificoEspecialidad.
            if (idsEspecialidades.Count > 0)
            {
                foreach (var espId in idsEspecialidades)
                {
                    var relacion = new CientificoEspecialidad
                    {
                        CientificoId = cientifico.Id,
                        EspecialidadId = espId
                    };
                    _context.CientificoEspecialidad.Add(relacion);
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetCientificoPorId), new { id = cientifico.Id }, new { mensaje = "Científico creado exitosamente.", id = cientifico.Id });
        }

        // 4. PUT: api/cientificos/{id} (Actualiza los datos del científico)
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

        // 5. DELETE: api/cientificos/{id} (Elimina un científico)
        // Se eliminan primero los registros relacionados (Descubrimientos, Investigaciones y
        // las relaciones en CientificoEspecialidad) para evitar errores de FK con SQL Server.
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCientifico(int id)
        {
            var cientifico = await _context.Cientificos.FindAsync(id);
            if (cientifico == null)
                return NotFound(new { mensaje = "Científico no encontrado." });

            // Elimina descubrimientos asociados.
            var descubrimientos = await _context.Descubrimientos
                .Where(d => d.CientificoId == id)
                .ToListAsync();
            if (descubrimientos.Count > 0)
                _context.Descubrimientos.RemoveRange(descubrimientos);

            // Elimina investigaciones asociadas.
            var investigaciones = await _context.Investigacion
                .Where(i => i.CientificoId == id)
                .ToListAsync();
            if (investigaciones.Count > 0)
                _context.Investigacion.RemoveRange(investigaciones);

            // Elimina las relaciones con especialidades.
            var relaciones = await _context.CientificoEspecialidad
                .Where(ce => ce.CientificoId == id)
                .ToListAsync();
            if (relaciones.Count > 0)
                _context.CientificoEspecialidad.RemoveRange(relaciones);

            _context.Cientificos.Remove(cientifico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 6. POST: api/cientificos/{id}/especialidades/{especialidadId} (Asigna una especialidad)
        [HttpPost("{id}/especialidades/{especialidadId}")]
        public async Task<IActionResult> AsignarEspecialidad(int id, int especialidadId)
        {
            var cientifico = await _context.Cientificos.FindAsync(id);
            if (cientifico == null) return NotFound(new { mensaje = "Científico no encontrado." });

            var especialidad = await _context.Especialidades.FindAsync(especialidadId);
            if (especialidad == null) return NotFound(new { mensaje = "Especialidad no encontrada." });

            // Verificar si ya existe la relación
            var existe = await _context.CientificoEspecialidad
                .AnyAsync(ce => ce.CientificoId == id && ce.EspecialidadId == especialidadId);

            if (existe) return BadRequest(new { mensaje = "El científico ya tiene asignada esta especialidad." });

            var relacion = new CientificoEspecialidad
            {
                CientificoId = id,
                EspecialidadId = especialidadId
            };

            _context.CientificoEspecialidad.Add(relacion);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Especialidad asignada correctamente al científico." });
        }

        // 7. DELETE: api/cientificos/{id}/especialidades/{especialidadId} (Remueve una especialidad)
        [HttpDelete("{id}/especialidades/{especialidadId}")]
        public async Task<IActionResult> RemoverEspecialidad(int id, int especialidadId)
        {
            var relacion = await _context.CientificoEspecialidad
                .FirstOrDefaultAsync(ce => ce.CientificoId == id && ce.EspecialidadId == especialidadId);

            if (relacion == null)
                return NotFound(new { mensaje = "La relación entre el científico y la especialidad no existe." });

            _context.CientificoEspecialidad.Remove(relacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}