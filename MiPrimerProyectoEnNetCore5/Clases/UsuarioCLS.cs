using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class UsuarioCLS
	{
		[Display(Name = "Id Usuario")]
		[DisplayName("Id Usuario")]
		[Required(ErrorMessage = "Seleccione usuario")]
		public int idUsuario { get; set; }

		[Display(Name = "Nombre Completo")]
		[DisplayName("Nombre Completo")]
		public string nombreCompleto { get; set; }

		[Display(Name = "Nombre Tipo Usuario")]
		[DisplayName("Nombre Tipo Usuario")]
		public string nombreTipoUsuario { get; set; }

		[Display(Name = "Nombre Usuario")]
		[DisplayName("Nombre Usuario")]
		[Required(ErrorMessage = "Seleccione nombre usuario")]
		public string nombreUsuario { get; set; }

		[Display(Name = "Id Tipo Usuario")]
		[DisplayName("Id Tipo Usuario")]
		[Required(ErrorMessage = "Seleccione tipo usuario")]
		public int? idTipoUsuario { get; set; }

		[Display(Name = "Contraseña")]
		[DisplayName("Contraseña")]
		[Required(ErrorMessage = "Seleccione contraseña")]
		public string contra { get; set; }

		[Display(Name = "Contraseña")]
		[DisplayName("Contraseña")]
		[Required(ErrorMessage = "Vuelva a introducir contraseña contraseña")]
		public string contra2 { get; set; }

		[Display(Name = "Id Persona")]
		[DisplayName("Id Persona")]
		[Required(ErrorMessage = "Seleccione persona")]
		public int? idPersona { get; set; }
	}
}
