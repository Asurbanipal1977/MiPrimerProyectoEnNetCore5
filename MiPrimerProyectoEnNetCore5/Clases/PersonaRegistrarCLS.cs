using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class PersonaRegistrarCLS
	{
		[Display(Name = "Id Persona")]
		[DisplayName("Id Persona")]
		public int IdPersona { get; set; }

		[Display(Name = "Ingrese Nombre")]
		[Required(ErrorMessage = "Debe introducir el nombre")]
		[DisplayName("Nombre")]
		public string Nombre { get; set; }
		[Display(Name = "Ingrese apellido paterno")]
		[DisplayName("Apellido Paterno")]
		[Required(ErrorMessage = "Debe introducir el apellido paterno")]

		public string Appaterno { get; set; }
		[Display(Name = "Ingrese apellido materno")]
		[DisplayName("Apellido materno")]
		[Required(ErrorMessage = "Debe introducir el apellido materno")]

		public string Apmaterno { get; set; }
		[Display(Name = "Nombre")]
		[DisplayName("Nombre")]
		public string NombreCompleto
		{
			get
			{
				return $"{this.Nombre} {this.Appaterno} {this.Apmaterno}";
			}
		}

		[DataType(DataType.Date, ErrorMessage = "El formato de fecha no es el correcto")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Required(ErrorMessage = "Debe introducir la fecha de nacimiento")]
		[Display(Name = "Fecha de Nacimiento")]
		[DisplayName("Fecha de Nacimiento")]
		public DateTime? Fechanacimiento { get; set; }

		[Display(Name = "Teléfono fijo")]
		[DisplayName("Teléfono fijo")]
		[Required(ErrorMessage = "Debe introducir el teléfono fijo")]
		[MinLength(9, ErrorMessage = "Longitud mínima de 9 caracteres")]
		public string Telefonofijo { get; set; }

		[Display(Name = "Teléfono móvil")]
		[DisplayName("Teléfono móvil")]
		[Required(ErrorMessage = "Debe introducir el teléfono móvil")]
		[MinLength(9, ErrorMessage = "Longitud mínima de 9 caracteres")]
		public string Telefonocelular { get; set; }

		[Display(Name = "Email")]
		[DisplayName("Email")]
		[Required(ErrorMessage = "Debe introducir el correo")]
		[DataType(DataType.EmailAddress, ErrorMessage = "El correo debe ser válido")]
		public string Email { get; set; }

		[Display(Name = "Seleccione una opción")]
		[Required(ErrorMessage = "Seleccione un sexo")]
		[DisplayName("Sexo")]
		public int? IdSexo { get; set; }


		[Display(Name = "Sexo")]
		public string Sexo { get; set; }

		public string MensajeErrorNombre { get; set; }

		public string MensajeErrorEmail { get; set; }

		[Display(Name = "Fecha de Nacimiento")]
		[DisplayName("Fecha de Nacimiento")]
		public string FechanacimientoCadena
		{
			get
			{
				return Fechanacimiento?.ToString("dd/MM/yyyy");
			}
		}

		public string FechanacimientoCadenaSet { get; set; }

		public string Foto { get; set; }

		[Display(Name = "Nombre Usuario")]
		[DisplayName("Nombre Usuario")]
		[Required(ErrorMessage = "Seleccione nombre usuario")]
		public string nombreUsuario { get; set; }

		[Display(Name = "Contraseña")]
		[DisplayName("Contraseña")]
		[Required(ErrorMessage = "Seleccione contraseña")]
		public string contra { get; set; }

		[Display(Name = "Contraseña")]
		[DisplayName("Contraseña")]
		[Required(ErrorMessage = "Vuelva a introducir contraseña contraseña")]
		public string contra2 { get; set; }


		public string nombreTipoUsuario { get; set; }
	}
}
