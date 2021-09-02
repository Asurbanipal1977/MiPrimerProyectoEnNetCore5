using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class PaginaTipoUsuarioCLS
	{

		public int idPaginaTipoUsuario { get; set; }

		[Display(Name = "Nombre Tipo Usuario")]
		[DisplayName("Nombre Tipo Usuario")]
		public string nombreTipoUsuario { get; set; }

		[Display(Name = "Nombre Pagina")]
		[DisplayName("Nombre Pagina")]
		public string nombrePagina { get; set; }
	}
}
