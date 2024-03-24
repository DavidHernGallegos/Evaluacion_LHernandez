using System;
using System.Collections.Generic;

namespace DL;

public partial class Ticket
{
    public string? IdTienda { get; set; }

    public string? IdRegistradora { get; set; }

    public DateTime? FechaHora { get; set; }

    public int? Ticket1 { get; set; }

    public decimal? Inpuesto { get; set; }

    public decimal? Total { get; set; }

    public TimeSpan? FechaHoraCreacion { get; set; }
}
