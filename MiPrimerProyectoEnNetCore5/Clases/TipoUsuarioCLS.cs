using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MiPrimerProyectoEnNetCore5.Models;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class TipoUsuarioCLS
	{
		[Display(Name="Id Tipo Usuario")]
		public int IdUsuario { get; set; }
		[Display(Name = "Nombre")]
		[Required(ErrorMessage ="Debe introducir el nombre")]
		public string Nombre { get; set; }
		[Display(Name = "Descripción")]
		[Required(ErrorMessage = "Debe introducir la descripción")]
		public string Descripcion { get; set; }

		public string MensajeErrorDescripcion { get; set; }
		public string MensajeErrorNombre { get; set; }

		public List<PaginaCLS> listaPaginas { get; set; } 
	}
}
