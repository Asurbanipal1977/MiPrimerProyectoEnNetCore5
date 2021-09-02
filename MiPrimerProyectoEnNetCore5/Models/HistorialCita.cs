using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class HistorialCita
    {
        public int Iidhistorialcita { get; set; }
        public int? Iidcita { get; set; }
        public int? Iidestado { get; set; }
        public int? Iidusuario { get; set; }
        public DateTime? Dfecha { get; set; }
        public string Vobservacion { get; set; }

        public virtual Cita IidcitaNavigation { get; set; }
        public virtual EstadoCita IidestadoNavigation { get; set; }
        public virtual Usuario IidusuarioNavigation { get; set; }
    }
}
