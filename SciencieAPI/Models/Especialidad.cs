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

        public virtual ICollection<Cientifico> Cientificos { get; set; } = new List<Cientifico>();
    }
}