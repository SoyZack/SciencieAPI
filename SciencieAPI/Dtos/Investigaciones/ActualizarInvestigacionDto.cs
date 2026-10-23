namespace SciencieAPI.Dtos.Investigaciones
{
    public class ActualizarInvestigacionDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string AreaEstudio { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty; // 'En Curso', 'Finalizada', 'Publicada'
    }
}
