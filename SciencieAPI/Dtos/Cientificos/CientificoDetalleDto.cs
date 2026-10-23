using SciencieAPI.Dtos.Descubrimientos;
using SciencieAPI.Dtos.Especialidades;
using SciencieAPI.Dtos.Investigaciones;

namespace SciencieAPI.Dtos.Cientificos
{
    public class CientificoDetalleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; } = string.Empty;

        // Listas completas que pide la rúbrica para el detalle por ID
        public List<EspecialidadDto> Especialidades { get; set; } = new();
        public List<DescubrimientosDto> Descubrimientos { get; set; } = new();
        public List<InvestigacionDto> Investigaciones { get; set; } = new();
    }
}
