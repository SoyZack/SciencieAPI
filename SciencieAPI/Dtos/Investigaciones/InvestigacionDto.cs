namespace SciencieAPI.Dtos.Investigaciones
{
    public class InvestigacionDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string AreaEstudio { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int CientificoId { get; set; }

    }

    public class CrearInvestigacionDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string AreaEstudio { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int CientificoId { get; set; }
    }
}
