﻿using System;
using System.Collections.Generic;

namespace CARNE.Models;

public partial class Inspeccione
{
    public int IdInspeccion { get; set; }

    public int? IdEstablecimiento { get; set; }

    public int? IdSolicitud { get; set; }

    public int? IdAdmin { get; set; }
    
    public int IdAdminInspector { get; set; }
    
    public DateTime? FechaInspeccion { get; set; }

    public int? Prioridad { get; set; }
    
    public string? Resultado { get; set; }

    public bool? FueEvaluada { get; set; }
    public virtual Admin IdAdminNavigation { get; set; } = null!;
   
    public virtual Establecimiento IdEstablecimientoNavigation { get; set; } = null!;

    public virtual Solicitud IdSolicitudNavigation { get; set; } = null!;
    

    public virtual ICollection<ResultadosInspeccion> ResultadosInspeccions { get; set; } = new List<ResultadosInspeccion>();
}
