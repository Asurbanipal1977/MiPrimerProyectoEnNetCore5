using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class Boton
    {
        public Boton()
        {
            TipoUsuarioPaginaBoton = new HashSet<TipoUsuarioPaginaBoton>();
        }

        public int Iidboton { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<TipoUsuarioPaginaBoton> TipoUsuarioPaginaBoton { get; set; }
    }
}
