using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SciencieAPI.Models
{
    public class Investigacion
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string AreaEstudio { get; set; } = string.Empty;

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required, StringLength(50)]
        public string Estado { get; set; } = string.Empty; // 'En curso', 'Finalizada', 'Publicada'

        // Lave foranea y navegacion hacia Cientifico
        public int CientificoId { get; set; }
        public Cientifico Cientifico { get; set; } = null!;
    }
}
