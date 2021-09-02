using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class Cita
    {
        public Cita()
        {
            CitaMedicamento = new HashSet<CitaMedicamento>();
            HistorialCita = new HashSet<HistorialCita>();
        }

        public int Iidcita { get; set; }
        public int? Iidusuario { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Estatura { get; set; }
        public string Examenfisico { get; set; }
        public string Examenlaboratorio { get; set; }
        public string Diagnostico { get; set; }
        public DateTime? Dfechacita { get; set; }
        public int? Iidestadocita { get; set; }
        public int? Iidsede { get; set; }
        public decimal? Precioatencion { get; set; }
        public decimal? Totalpagar { get; set; }
        public DateTime? Dfechainicioenfermedad { get; set; }
        public int? Iiddoctorasignado { get; set; }
        public int? Bhabilitado { get; set; }
        public int? Iidpersona { get; set; }
        public string Descripcionenfermedad { get; set; }

        public virtual Doctor IiddoctorasignadoNavigation { get; set; }
        public virtual EstadoCita IidestadocitaNavigation { get; set; }
        public virtual Persona IidpersonaNavigation { get; set; }
        public virtual Sede IidsedeNavigation { get; set; }
        public virtual Usuario IidusuarioNavigation { get; set; }
        public virtual ICollection<CitaMedicamento> CitaMedicamento { get; set; }
        public virtual ICollection<HistorialCita> HistorialCita { get; set; }
    }
}
