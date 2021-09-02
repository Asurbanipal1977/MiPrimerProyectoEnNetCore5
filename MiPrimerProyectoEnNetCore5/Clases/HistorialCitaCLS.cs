using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class HistorialCitaCLS
	{
		public int idHistorialCita { get; set; }
		public int idCita { get; set; }
		public string estadoCita { get; set; }

		public string fechaEstado { get; set; }
		public string nombreUsuario { get; set; }
	}
}
