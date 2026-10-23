using Microsoft.EntityFrameworkCore;
using SciencieAPI.Models;

namespace SciencieAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Definicion de las tablas (DbSets) segun tu script SQL.
        // Se inicializan con null! para satisfacer el compilador con Nullable habilitado.
        public DbSet<Cientifico> Cientificos { get; set; } = null!;
        public DbSet<Especialidad> Especialidades { get; set; } = null!;
        public DbSet<CientificoEspecialidad> CientificoEspecialidad { get; set; } = null!;

        public DbSet<Descubrimiento> Descubrimientos { get; set; } = null!;
        public DbSet<Investigacion> Investigacion { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la llave primaria compuesta para la tabla intermedia Muchos a Muchos
            modelBuilder.Entity<CientificoEspecialidad>()
                .HasKey(ce => new { ce.CientificoId, ce.EspecialidadId });

            // Configuración de la relación Cientifico -> CientificoEspecialidad
            modelBuilder.Entity<CientificoEspecialidad>()
                .HasOne(ce => ce.Cientifico)
                .WithMany(c => c.CientificoEspecialidades)
                .HasForeignKey(ce => ce.CientificoId);

            // Configuración de la relación Especialidad -> CientificoEspecialidad
            modelBuilder.Entity<CientificoEspecialidad>()
                .HasOne(ce => ce.Especialidad)
                .WithMany(e => e.CientificoEspecialidades)
                .HasForeignKey(ce => ce.EspecialidadId);

            // Configuraciones adicionales de restricciones (opcional pero recomendado)
            modelBuilder.Entity<Cientifico>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
