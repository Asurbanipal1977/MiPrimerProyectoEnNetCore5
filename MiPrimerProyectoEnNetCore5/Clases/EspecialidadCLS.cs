using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class EspecialidadCLS
	{
		[Display(Name = "Id Especialidad")]
		[DisplayName("Id Especialidad")]
		public int idEspecialidad { get; set; }

		[Display(Name = "Nombre Especialidad")]
		[DisplayName("Nombre Especialidad")]
		[Required(ErrorMessage = "Introduzca el nombre de la especialidad")]
		public string nombre { get; set; }

		[Display(Name = "Descripción")]
		[DisplayName("Descripción")]
		[Required(ErrorMessage = "Introduzca la descripción de la especialidad")]
		public string descripcion { get; set; }

		public string MensajeError { get; set; }
	}
}
