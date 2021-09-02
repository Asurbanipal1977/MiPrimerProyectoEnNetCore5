using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class PaginaCLS
	{
		[Display(Name = "Id Pagina")]
		public int IdPagina { get; set; }

		[Display(Name = "Mensaje")]
		[Required(ErrorMessage = "Introduzca el mensaje")]
		public string Mensaje { get; set; }

		[Display(Name = "Acción")]
		[Required(ErrorMessage = "Introduzca la acción")]
		public string Accion { get; set; }

		[Display(Name = "Controlador")]
		[Required(ErrorMessage = "Introduzca el controlador")]
		[MinLength(3, ErrorMessage = "La longitud mínima es 3")]
		[MaxLength(100, ErrorMessage = "La longitud máxima es 100")]
		public string Controlador { get; set; }

		public string MensajeError { get; set; }

		public int IdBoton { get; set; }

		public int IdVista { get; set; }
	}
}
