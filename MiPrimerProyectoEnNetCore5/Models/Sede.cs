using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class Sede
    {
        public Sede()
        {
            Cita = new HashSet<Cita>();
            Doctor = new HashSet<Doctor>();
        }

        public int Iidsede { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<Cita> Cita { get; set; }
        public virtual ICollection<Doctor> Doctor { get; set; }
    }
}
