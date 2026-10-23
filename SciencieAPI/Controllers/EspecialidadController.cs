using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SciencieAPI.Data;
using SciencieAPI.Dtos.Especialidades;
using SciencieAPI.Models;

namespace SciencieAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EspecialidadesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/especialidades (Obtiene el catálogo de especialidades)[cite: 2]
        [HttpGet]
        public async Task<IActionResult> GetEspecialidades()
        {
            var especialidades = await _context.Especialidades.ToListAsync();
            var especialidadesDto = _mapper.Map<List<EspecialidadDto>>(especialidades);
            return Ok(especialidadesDto);
        }

        // POST: api/especialidades (Crea una nueva especialidad)[cite: 2]
        [HttpPost]
        public async Task<IActionResult> CrearEspecialidad([FromBody] CrearEspecialidadDto crearDto)
        {
            var especialidad = _mapper.Map<Especialidad>(crearDto);

            _context.Especialidades.Add(especialidad);
            await _context.SaveChangesAsync();

            var especialidadDto = _mapper.Map<EspecialidadDto>(especialidad);
            return CreatedAtAction(nameof(GetEspecialidades), new { id = especialidad.Id }, especialidadDto);
        }
    }
}