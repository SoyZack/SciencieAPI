using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SciencieAPI.Models;

public partial class DescubrimientosContext : DbContext
{
    public DescubrimientosContext()
    {
    }

    public DescubrimientosContext(DbContextOptions<DescubrimientosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cientifico> Cientificos { get; set; }

    public virtual DbSet<Descubrimiento> Descubrimientos { get; set; }

    public virtual DbSet<Especialidad> Especialidades { get; set; }

    public virtual DbSet<Investigacion> Investigaciones { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cientifico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cientifi__3214EC07BDDEB7B4");

            entity.HasIndex(e => e.Email, "UQ__Cientifi__A9D1053435547525").IsUnique();

            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasMany(d => d.Especialidades).WithMany(p => p.Cientificos)
                .UsingEntity<Dictionary<string, object>>(
                    "CientificoEspecialidad",
                    r => r.HasOne<Especialidad>().WithMany()
                        .HasForeignKey("EspecialidadId")
                        .HasConstraintName("FK__Cientific__Espec__5070F446"),
                    l => l.HasOne<Cientifico>().WithMany()
                        .HasForeignKey("CientificoId")
                        .HasConstraintName("FK__Cientific__Cient__4F7CD00D"),
                    j =>
                    {
                        j.HasKey("CientificoId", "EspecialidadId").HasName("PK__Cientifi__AA8141A0C5BDC7D3");
                        j.ToTable("CientificoEspecialidad");
                    });
        });

        modelBuilder.Entity<Descubrimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Descubri__3214EC07C78E8DF2");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Lugar)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Cientifico).WithMany(p => p.Descubrimientos)
                .HasForeignKey(d => d.CientificoId)
                .HasConstraintName("FK__Descubrim__Cient__534D60F1");
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Especial__3214EC07FF894F91");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Investigacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Investig__3214EC07CD7AFA56");

            entity.Property(e => e.AreaEstudio)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Cientifico).WithMany(p => p.Investigaciones)
                .HasForeignKey(d => d.CientificoId)
                .HasConstraintName("FK__Investiga__Cient__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
