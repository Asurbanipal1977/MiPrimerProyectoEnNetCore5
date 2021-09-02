using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class MedicamentoCLS
	{
		[Display(Name = "Id Medicamento")]
		[DisplayName("Id Medicamento")]
		public int IdMedicamento { get; set; }
		[Display(Name = "Nombre del medicamento")]
		[DisplayName("Nombre del medicamento")]
		[Required(ErrorMessage = "Ingrese el nombre del medicamento") ]
		public string Nombre { get; set; }

		[Display(Name = "Precio")]
		[DisplayName("Precio")]
		[Required(ErrorMessage = "Ingrese el precio")]
		public decimal? Precio { get; set; }

		[Required(ErrorMessage = "Ingrese el stock")]
		[Display(Name = "Stock")]
		[DisplayName("Stock")]
		[Range(0,10000,ErrorMessage = "Debe estar entre 0 y 10000")]
		public int? Stock { get; set; }

		[Display(Name = "Nombre Forma Farmaceutica")]
		[DisplayName("Nombre Forma Farmaceutica")]
		public string NombreForma { get; set; }

		[Display(Name = "Seleccione Forma Farmaceutica")]
		[Required(ErrorMessage = "Seleccione la forma farmaceutica")]
		public int? IdFormaFarmaceutica { get; set; }

		[Display(Name = "Concentración")]
		public string Concentracion { get; set; }

		[Display(Name = "Presentación")]
		public string Presentacion { get; set; }
	}
}
