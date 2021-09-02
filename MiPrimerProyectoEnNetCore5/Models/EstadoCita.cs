using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class EstadoCita
    {
        public EstadoCita()
        {
            Cita = new HashSet<Cita>();
            HistorialCita = new HashSet<HistorialCita>();
        }

        public int Iidestado { get; set; }
        public string Vnombre { get; set; }
        public string Vdescripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<Cita> Cita { get; set; }
        public virtual ICollection<HistorialCita> HistorialCita { get; set; }
    }
}
