namespace SciencieAPI.Models
{
    public class CientificoEspecialidad
    {
        public int CientificoId { get; set; }
        public Cientifico Cientifico { get; set; } = null!;

        public int EspecialidadId { get; set; }
        // EF Core llena esta navegación al cargar la relación.
        public Especialidad Especialidad { get; set; } = null!;
    }
}
