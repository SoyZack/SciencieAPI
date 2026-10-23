using System.ComponentModel.DataAnnotations;

namespace SciencieAPI.Models
{
    public class Especialidad
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Descripcion { get; set; }

        // Propiedad de la navegacion para la relacion Muchos a Muchos
        public ICollection<CientificoEspecialidad> CientificoEspecialidades { get; set; } = new List<CientificoEspecialidad>();
    }
}
