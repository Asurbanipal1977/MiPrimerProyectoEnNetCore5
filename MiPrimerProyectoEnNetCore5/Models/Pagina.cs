using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class Pagina
    {
        public Pagina()
        {
            TipoUsuarioPagina = new HashSet<TipoUsuarioPagina>();
        }

        public int Iidpagina { get; set; }
        public string Mensaje { get; set; }
        public string Accion { get; set; }
        public string Controlador { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<TipoUsuarioPagina> TipoUsuarioPagina { get; set; }
    }
}
