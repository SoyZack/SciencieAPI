using System.ComponentModel.DataAnnotations;

namespace SciencieAPI.Models
{
    public class Descubrimiento
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        // La columna en SQL Server se llama "Descripcion".
        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaDescubrimiento { get; set; }

        [Required, StringLength(150)]
        public string Lugar { get; set; } = string.Empty;

        // Llave foranea y navegacion hacia Cientifico
        public int CientificoId { get; set; }
        public Cientifico Cientifico { get; set; } = null!;
    }
}
