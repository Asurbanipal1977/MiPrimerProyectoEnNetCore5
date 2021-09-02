using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class DoctorCLS
	{
		[Display(Name = "Id Doctor")]
		[DisplayName("Id doctor")]
		[Required(ErrorMessage = "Seleccione id Doctor")]
		public int idDoctor { get; set; }

		[Display(Name = "Nombre Completo")]
		[DisplayName("Nombre Completo")]
		public string nombreCompleto { get; set; }

		[Display(Name = "Nombre Sede")]
		[DisplayName("Nombre Sede")]
		public string nombreSede { get; set; }

		[Display(Name = "Nombre Especialidad")]
		[DisplayName("Nombre Especialidad")]
		public string especialidad { get; set; }


		[Display(Name = "Id Especialidad")]
		[DisplayName("Id Especialidad")]
		[Required(ErrorMessage = "Seleccione la especialidad")]
		public int? idEspecialidad { get; set; }

		[Display(Name = "Id Sede")]
		[DisplayName("Id Sede")]
		[Required(ErrorMessage = "Seleccione sede")]
		public int? idSede { get; set; }

		[Display(Name = "Id Persona")]
		[DisplayName("Id Persona")]
		[Required(ErrorMessage = "Seleccione persona")]
		public int? idPersona { get; set; }


		[Display(Name = "Sueldo")]
		[DisplayName("Sueldo")]
		[Required(ErrorMessage = "Indique sueldo")]
		public decimal? sueldo { get; set; }


		[Display(Name = "Fecha Contrato")]
		[DisplayName("Fecha Contrato")]
		[Required(ErrorMessage = "Seleccione fecha contrato")]
		public DateTime? fechaContrato { get; set; }

	}
}
