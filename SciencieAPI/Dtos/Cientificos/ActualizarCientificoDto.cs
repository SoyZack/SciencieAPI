namespace SciencieAPI.Dtos.Cientificos
{
    public class ActualizarCientificoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;
        // Se mantiene el nombre "FechaNaciemiento" para preservar el contrato JSON existente.
        public DateTime FechaNaciemiento { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
