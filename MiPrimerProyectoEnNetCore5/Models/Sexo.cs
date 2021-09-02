using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class Sexo
    {
        public Sexo()
        {
            Persona = new HashSet<Persona>();
        }

        public int Iidsexo { get; set; }
        public string Nombre { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<Persona> Persona { get; set; }
    }
}
