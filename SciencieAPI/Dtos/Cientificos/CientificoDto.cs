using SciencieAPI.Dtos.Especialidades;

namespace SciencieAPI.Dtos.Cientificos
{

    // DTO para mostrar la informacion del cientifico al hacer GET
    public class CientificoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; } = string.Empty;

        // Listas opcionales para mostrar especialidades o descubrimientos asociados si quiere requeridos 
        public List<EspecialidadDto> Especialidades { get; set; } = new();
    }

    // DTO para registrar un nuevo científico (POST)
    public class CrearCientificoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;
        // Se mantiene el nombre "FechaNaciemiento" para preservar el contrato JSON existente.
        public DateTime FechaNaciemiento { get; set; }
        public string Email { get; set; } = string.Empty;

        // Permite asignar especialidades desde el registro inicial si se desea
        public List<int> EspecialidadIds { get; set; } = new();
    }

}
