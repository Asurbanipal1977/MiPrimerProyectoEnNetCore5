using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class RevisarCitaCLS
	{
		[Display(Name = "Id Cita")]
		[DisplayName("Id cita")]
		public int idCita { get; set; }

		[Display(Name = "Id Doctor")]
		[DisplayName("Id doctor")]
		[Required(ErrorMessage = "Debe introducir el id de Doctor")]
		public int? idDoctor { get; set; }

		[Display(Name = "Fecha cita")]
		[DisplayName("Fecha cita")]
		[Required(ErrorMessage = "Debe introducir la fecha de la cita")]
		public DateTime? fechaCita { get; set; }

		[Display(Name = "Precio")]
		[DisplayName("Precio")]
		[Required(ErrorMessage = "Debe introducir el precio de la cita")]
		public decimal? precioCita { get; set; }
	}
}
