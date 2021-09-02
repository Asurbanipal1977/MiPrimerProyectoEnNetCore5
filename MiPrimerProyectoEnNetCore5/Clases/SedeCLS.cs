using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class SedeCLS
	{
		[Display(Name = "Id Especialidad")]
		public int idSede { get; set; }

		[Display(Name = "Nombre")]
		[Required(ErrorMessage = "Debe introducir el nombre")]
		public string Nombre { get; set; }

		[Display(Name = "Dirección")]
		[Required(ErrorMessage = "Debe introducir la dirección")]
		public string Direccion { get; set; }

	}
}
