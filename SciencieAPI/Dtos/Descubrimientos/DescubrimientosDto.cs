namespace SciencieAPI.Dtos.Descubrimientos
{
    public class DescubrimientosDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaDescubrimiento { get; set; }
        public string Lugar { get; set; } = string.Empty;
        public int CientificoId { get; set; }
    }

    public class CrearDescubrimientoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaDescubrimiento { get; set;  }
        public string Lugar { get; set; } = string.Empty;
        public int CientificoId { get; set; }
    }
}
