using System;
using System.Collections.Generic;

namespace SciencieAPI.Models;

public partial class Descubrimiento
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateOnly FechaDescubrimiento { get; set; }

    public string Lugar { get; set; } = null!;

    public int CientificoId { get; set; }

    public virtual Cientifico Cientifico { get; set; } = null!;
}
