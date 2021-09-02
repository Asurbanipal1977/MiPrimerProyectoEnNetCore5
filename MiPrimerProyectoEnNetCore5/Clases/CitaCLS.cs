using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class CitaCLS
	{
		[Display(Name = "Id Cita")]
		[DisplayName("Id cita")]
		public int idCita { get; set; }

		[Display(Name = "Id Persona")]
		[DisplayName("Id persona")]
		[Required(ErrorMessage = "Seleccione id Persona")]
		public int? idPersona { get; set; }

		[Display(Name = "Fecha enfermedad")]
		[DisplayName("Fecha Enfermedad")]
		[Required(ErrorMessage = "Seleccione fecha enfermedad")]
		public DateTime? fechaEnfermedad { get; set; }

		[Display(Name = "Id Sede")]
		[DisplayName("Id sede")]
		[Required(ErrorMessage = "Seleccione id Sede")]
		public int? idSede { get; set; }


		[Display(Name = "Descripcion")]
		[DisplayName("Descripcion")]
		[Required(ErrorMessage = "Indique Descripcion")]
		public string descripcionEnfermedad { get; set; }


		public string nombreCompleto { get; set; }
		public string nombreUsuario { get; set; }

		public string estadoCita { get; set; }
		public int idEstadoCita { get; set; }
		public string fechaEnfermedadCadena { get; set; }
	}
}
