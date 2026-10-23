using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SciencieAPI.Models
{
    public class Cientifico
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        // La columna "Nacionalidad" ya existe en la tabla Cientificos de SQL Server.
        [Required, StringLength(50)]
        public string Nacionalidad { get; set; } = string.Empty;

        // La columna real en SQL Server se llama "FechaNaciemiento" (con el typo),
        // por eso se indica el nombre de columna para que EF Core la encuentre.
        [Required]
        [Column("FechaNaciemiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required, StringLength(150)]
        public string Email { get; set; } = string.Empty;

        // Propiedades de navegacion (Relaciones)
        public ICollection<CientificoEspecialidad> CientificoEspecialidades { get; set; } = new List<CientificoEspecialidad>();
        public ICollection<Descubrimiento> Descubrimientos { get; set; } = new List<Descubrimiento>();
        public ICollection<Investigacion> Investigaciones { get; set; } = new List<Investigacion>();
    }
}
