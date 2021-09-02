using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class TipoUsuario
    {
        public TipoUsuario()
        {
            TipoUsuarioPagina = new HashSet<TipoUsuarioPagina>();
            Usuario = new HashSet<Usuario>();
        }

        public int Iidtipousuario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<TipoUsuarioPagina> TipoUsuarioPagina { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
