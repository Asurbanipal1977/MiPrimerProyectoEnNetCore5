using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class PacienteCLS
	{
		[Display(Name = "Id Paciente")]
		[DisplayName("Is paciente")]
		[Required(ErrorMessage = "Seleccione id Paciente")]
		public int idPaciente { get; set; }

		[Display(Name = "Nombre Completo")]
		[DisplayName("Nombre Completo")]
		public string nombreCompleto { get; set; }

		[Display(Name = "Tipo de sangre")]
		[DisplayName("Tipo de sangre")]
		public string tipoSangre { get; set; }

		[Display(Name = "Alergia")]
		[DisplayName("Alergia")]
		public string alergia { get; set; }


		[Display(Name = "Id tipo sangre")]
		[DisplayName("Id tipo sangre")]
		[Required(ErrorMessage = "Seleccione tipo de sangre")]
		public int? idTipoSangre{ get; set; }

		[Display(Name = "Enfermedades crónicas")]
		[DisplayName("Enfermedades crónica")]
		[MaxLength(200,ErrorMessage = "Enfermedades crónica tiene un máximo de 200 caracteres")]
		public string enfermedadesCronicas { get; set; }

		[Display(Name = "Cuadro vacunas")]
		[DisplayName("Cuadro vacunas")]
		[MaxLength(200, ErrorMessage = "Cuadro vacunas tiene un máximo de 200 caracteres")]
		public string cuadroVacunas { get; set; }

		[Display(Name = "Antecedentes Quirúrgicos")]
		[DisplayName("Antecedentes Quirúrgicos")]
		[MaxLength(200, ErrorMessage = "Antecedentes Quirúrgicos tiene un máximo de 200 caracteres")]
		public string antecedentesQuirurgicos { get; set; }

		[Display(Name = "Id Persona")]
		[DisplayName("Id Persona")]
		[Required(ErrorMessage = "Seleccione persona")]
		public int? idPersona { get; set; }
	}
}
