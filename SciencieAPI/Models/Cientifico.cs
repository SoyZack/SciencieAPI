using System;
using System.Collections.Generic;

namespace SciencieAPI.Models;

public partial class Cientifico
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Nacionalidad { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<Descubrimiento> Descubrimientos { get; set; } = new List<Descubrimiento>();

    public virtual ICollection<Investigacion> Investigaciones { get; set; } = new List<Investigacion>();

    public virtual ICollection<Especialidad> Especialidades { get; set; } = new List<Especialidad   >();
}
