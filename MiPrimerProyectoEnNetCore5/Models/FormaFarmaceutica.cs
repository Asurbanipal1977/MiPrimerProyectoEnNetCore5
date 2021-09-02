using System;
using System.Collections.Generic;

namespace MiPrimerProyectoEnNetCore5.Models
{
    public partial class FormaFarmaceutica
    {
        public FormaFarmaceutica()
        {
            Medicamento = new HashSet<Medicamento>();
        }

        public int Iidformafarmaceutica { get; set; }
        public string Nombre { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<Medicamento> Medicamento { get; set; }
    }
}
