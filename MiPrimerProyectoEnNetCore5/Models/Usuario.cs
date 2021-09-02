using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Cita = new HashSet<Cita>();
            HistorialCita = new HashSet<HistorialCita>();
            Persona = new HashSet<Persona>();
        }

        public int Iidusuario { get; set; }
        public int Iidtipousuario { get; set; }
        public string Nombreusuario { get; set; }
        public string Contraseña { get; set; }
        public int? Bhabilitado { get; set; }
        public int? Iidpersona { get; set; }

        public virtual Persona IidpersonaNavigation { get; set; }
        public virtual TipoUsuario IidtipousuarioNavigation { get; set; }
        public virtual ICollection<Cita> Cita { get; set; }
        public virtual ICollection<HistorialCita> HistorialCita { get; set; }
        public virtual ICollection<Persona> Persona { get; set; }
    }
}
