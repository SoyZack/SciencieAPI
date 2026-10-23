namespace SciencieAPI.Dtos.Descubrimientos
{
    public class ActualizarDescubrimientoDto
    {
        public string Titulo { get; set; } = string.Empty;
        // Coincide con la propiedad "Descripcion" del modelo y de la tabla.
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaDescubrimiento { get; set; }
        public string Lugar { get; set; } = string.Empty;
    }
}
