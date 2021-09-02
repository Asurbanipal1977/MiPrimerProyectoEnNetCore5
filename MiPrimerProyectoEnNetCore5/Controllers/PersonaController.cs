using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiPrimerProyectoEnNetCore5.Clases;
using MiPrimerProyectoEnNetCore5.Models;

namespace MiPrimerProyectoEnNetCore5.Controllers
{
	public class PersonaController : BaseController
	{
		public static List<PersonaCLS> lista;

		public FileResult exportar(string[] nombrePropiedades, string tipoReporte)
		{
			switch (tipoReporte)
			{
				case "Excel":
					byte[] buffer = exportarExcelDatos(nombrePropiedades, lista);
					return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
				case "Word":
					byte[] bufferWord = exportarWordDatos(nombrePropiedades, lista);
					return File(bufferWord, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
			}

			return null;
		}

		public string exportarPdfDatos(string[] nombrePropiedades)
		{
			byte[] buffer = exportarPdfDatos(nombrePropiedades, lista);
			string cadena = Convert.ToBase64String(buffer);
			cadena = "data:application/pdf;base64," + cadena;

			return cadena;
		}

		public void LlenarSexo()
		{
			List<SelectListItem> listSexo = new List<SelectListItem>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listSexo = bdContext.Sexo.Where(s => s.Bhabilitado == 1)
				.Select(s => new SelectListItem
				{
					Value = s.Iidsexo.ToString(),
					Text = s.Nombre
				}).ToList();
				listSexo.Insert(0, new SelectListItem { Value="", Text="--Seleccione sexo--"});
			}
			ViewBag.listSexo = listSexo;
		}

		public List<PersonaCLS> Listar(PersonaCLS personaCLS)
		{
			List<PersonaCLS> listPersona = new List<PersonaCLS>();

			int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));
			int idVista = Generic.ObtenerTipoVista("Persona", idUsuario);

			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				int idPersona = (int) bdContext.Usuario.Where(p => p.Iidusuario == idUsuario).Select(p => p.Iidpersona).First();
				//si idVista es 1, es total
				listPersona = (from p in bdContext.Persona
							   join s in bdContext.Sexo on p.Iidsexo equals s.Iidsexo
							   where
							   (personaCLS.bDoctor==null ? p.Iidpersona == p.Iidpersona : p.Bdoctor == personaCLS.bDoctor) &&
							   (string.IsNullOrWhiteSpace(personaCLS.NombreCompleto) ? p.Iidpersona == p.Iidpersona :
								(p.Nombre + " " + p.Appaterno + " " + p.Apmaterno).Trim().ToUpper().Contains(personaCLS.NombreCompleto.Trim().ToUpper())) &&
							    p.Bhabilitado == 1	&& (idVista == 1 ? p.Iidusuario == p.Iidusuario : (p.Iidusuario == idUsuario || p.Iidpersona== idPersona))
							   select new PersonaCLS
							   {
								   IdPersona = p.Iidpersona,
								   Nombre = p.Nombre,
								   Appaterno = p.Appaterno,
								   Apmaterno = p.Apmaterno,
								   Email = p.Email,
								   Sexo = s.Nombre,
								   Fechanacimiento = p.Fechanacimiento
							   }).ToList();

				lista = listPersona;
				return listPersona;
			}
		}


		public List<PersonaCLS> ListarPersonaDoctor(PersonaCLS personaCLS)
		{
			List<PersonaCLS> listPersona = new List<PersonaCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listPersona = bdContext.Persona.Where(persona => persona.Bhabilitado == 1 &&
				persona.Bdoctor == 0 &&
				(personaCLS.NombreCompleto == null ? persona.Iidpersona == persona.Iidpersona :
				 (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Trim().ToUpper().Contains(personaCLS.NombreCompleto.Trim().ToUpper())))
					.Select (persona => new PersonaCLS
						{
							IdPersona = persona.Iidpersona,
							Nombre = persona.Nombre,
							Appaterno = persona.Appaterno,
							Apmaterno = persona.Apmaterno,
							Email = persona.Email,
							Fechanacimiento = persona.Fechanacimiento
						}).ToList();
				lista = listPersona;
				return listPersona;
			}
		}

		public List<PersonaCLS> ListarPersonaPaciente(PersonaCLS personaCLS)
		{
			List<PersonaCLS> listPersona = new List<PersonaCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listPersona = bdContext.Persona.Where(persona => persona.Bhabilitado == 1 &&
				persona.Bpaciente == 0 &&
				(personaCLS.NombreCompleto == null ? persona.Iidpersona == persona.Iidpersona :
				 (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Trim().ToUpper().Contains(personaCLS.NombreCompleto.Trim().ToUpper())))
					.Select(persona => new PersonaCLS
					{
						IdPersona = persona.Iidpersona,
						Nombre = persona.Nombre,
						Appaterno = persona.Appaterno,
						Apmaterno = persona.Apmaterno,
						Email = persona.Email,
						Fechanacimiento = persona.Fechanacimiento
					}).ToList();
				lista = listPersona;
				return listPersona;
			}
		}

		public List<PersonaCLS> ListarPersonaUsuario(PersonaCLS personaCLS)
		{
			List<PersonaCLS> listPersona = new List<PersonaCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listPersona = bdContext.Persona.Where(persona => persona.Bhabilitado == 1 &&
				persona.Btieneusuario == 0 &&
				(personaCLS.NombreCompleto == null ? persona.Iidpersona == persona.Iidpersona :
				 (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Trim().ToUpper().Contains(personaCLS.NombreCompleto.Trim().ToUpper())))
					.Select(persona => new PersonaCLS
					{
						IdPersona = persona.Iidpersona,
						Nombre = persona.Nombre,
						Appaterno = persona.Appaterno,
						Apmaterno = persona.Apmaterno,
						Email = persona.Email,
						Fechanacimiento = persona.Fechanacimiento
					}).ToList();
				lista = listPersona;
				return listPersona;
			}
		}


		public List<PersonaCLS> Filtrar(PersonaCLS personaCLS)
		{
			List<PersonaCLS> listPersona = new List<PersonaCLS>();
			int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));
			int idVista = Generic.ObtenerTipoVista("Persona", idUsuario);
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listPersona = (from p in bdContext.Persona
							   join s in bdContext.Sexo on p.Iidsexo equals s.Iidsexo
							   where p.Bhabilitado == 1
								 && p.Iidsexo == personaCLS.IdSexo &&
								 (idVista == 1 ? p.Iidusuario == p.Iidusuario : p.Iidusuario == idUsuario)
								 select new PersonaCLS
								 {
									 IdPersona = p.Iidpersona,
									 Nombre = p.Nombre,
									 Appaterno = p.Appaterno,
									 Apmaterno = p.Apmaterno,
									 Email = p.Email,
									 Sexo = s.Nombre,
									 Fechanacimiento = p.Fechanacimiento
								 }).ToList();

				lista = listPersona;
				return listPersona;
			}
		}


		public IActionResult Index(PersonaCLS personaCLS)
		{
			//	List<PersonaCLS> listPersona = new List<PersonaCLS>();
			//	LlenarSexo();
			//	using (BDHospitalContext bdContext = new BDHospitalContext())
			//	{
			//		if (personaCLS.IdSexo == 0 || personaCLS.IdSexo == null)
			//		{
			//			listPersona = bdContext.Persona.Where(persona => persona.Bhabilitado == 1).Join(bdContext.Sexo, persona => persona.Iidsexo, sexo => sexo.Iidsexo,
			//				(persona, sexo) => new PersonaCLS
			//				{
			//					IdPersona = persona.Iidpersona,
			//					Nombre = persona.Nombre,
			//					Appaterno = persona.Appaterno,
			//					Apmaterno = persona.Apmaterno,
			//					Email = persona.Email,
			//					Sexo = sexo.Nombre
			//				}).ToList();
			//		}
			//		else
			//		{
			//			listPersona = bdContext.Persona.Where(persona => persona.Bhabilitado == 1 && persona.Iidsexo==personaCLS.IdSexo).Join(bdContext.Sexo, persona => persona.Iidsexo, sexo => sexo.Iidsexo,
			//				(persona, sexo) => new PersonaCLS
			//				{
			//					IdPersona = persona.Iidpersona,
			//					Nombre = persona.Nombre,
			//					Appaterno = persona.Appaterno,
			//					Apmaterno = persona.Apmaterno,
			//					Email = persona.Email,
			//					Sexo = sexo.Nombre
			//				}).ToList();
			//		}
			//	}

			int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));
			int idVista = Generic.ObtenerTipoVista("Persona",idUsuario);

			LlenarSexo();
			return View();
		}

		public IActionResult Agregar()
		{
			LlenarSexo();
			return View();
		}


		public string GuardarDatos(PersonaCLS personaCLS)
		{
			string resultado = "";
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					int nvecesNombre = (personaCLS.IdPersona == 0) ? bDHospitalContext.Persona
							.Where(p => p.Nombre.Trim().ToUpper() + " " + p.Appaterno.Trim().ToUpper() + " " + p.Apmaterno.Trim().ToUpper() ==
							personaCLS.NombreCompleto.Trim().ToUpper() && p.Bhabilitado == 1).Count() :
							bDHospitalContext.Persona
							.Where(p => p.Nombre.Trim().ToUpper() + " " + p.Appaterno.Trim().ToUpper() + " " + p.Apmaterno.Trim().ToUpper() ==
							personaCLS.NombreCompleto.Trim().ToUpper() &&
							p.Iidpersona != personaCLS.IdPersona && p.Bhabilitado == 1).Count();

					int nvecesEmail = (personaCLS.IdPersona == 0) ? bDHospitalContext.Persona
							.Where(p => p.Email.Trim().ToUpper() == personaCLS.Email.Trim().ToUpper() && p.Bhabilitado == 1).Count() :
							bDHospitalContext.Persona
							.Where(p => p.Email.Trim().ToUpper() == personaCLS.Email.Trim().ToUpper() &&
							p.Iidpersona != personaCLS.IdPersona && p.Bhabilitado == 1).Count();

					if (!ModelState.IsValid || nvecesNombre > 0 || nvecesEmail > 0)
					{
						var listElementos = (from model in ModelState.Values
											 from errors in model.Errors
											 select errors.ErrorMessage).ToList();

						resultado = "<ul class='list-group'>";
						foreach (var item in listElementos)
						{
							resultado += "<li class='list-group-item list-group-item-danger'>" + item + "</li>";
						}

						if (nvecesNombre > 0) resultado += "<li class='list-group-item list-group-item-danger'>El nombre ya está repetido</li>";
						if (nvecesEmail > 0) resultado += "<li class='list-group-item list-group-item-danger'>El email está repetido</li>";

						resultado += "</ul>";						
					}
					else
					{
						Persona persona = (personaCLS.IdPersona != 0) ?
							bDHospitalContext.Persona
								.Where(p => p.Iidpersona == personaCLS.IdPersona).First() : new Persona();

						if (personaCLS.IdPersona != 0)
							persona.Iidpersona = personaCLS.IdPersona;
						persona.Nombre = personaCLS.Nombre;
						persona.Appaterno = personaCLS.Appaterno;
						persona.Apmaterno = personaCLS.Apmaterno;
						persona.Telefonofijo = personaCLS.Telefonofijo;
						persona.Telefonocelular = personaCLS.Telefonocelular;
						persona.Fechanacimiento = personaCLS.Fechanacimiento;
						persona.Email = personaCLS.Email;
						persona.Iidsexo = personaCLS.IdSexo;
						persona.Bhabilitado = 1;
						persona.Foto = personaCLS.Foto;
						persona.Iidusuario = int.Parse(HttpContext.Session.GetString("usuario")); //Usuario que registra

						if (personaCLS.IdPersona == 0)
						{
							persona.Bdoctor = 0;
							persona.Bpaciente = 0;
							persona.Btieneusuario = 0;
							bDHospitalContext.Persona.Add(persona);
						}

						bDHospitalContext.SaveChanges();
						resultado = "OK";
					}
				}
			}
			catch (Exception ex)
			{
				resultado = ex.Message;
			}

			return resultado;
		}

		[HttpPost]
		public IActionResult Guardar(PersonaCLS personaCLS)
		{
			string vista = (personaCLS.IdPersona == 0) ? "Agregar" : "Editar";
			try
			{
				LlenarSexo();
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					int nvecesNombre = (personaCLS.IdPersona == 0) ? bDHospitalContext.Persona
							.Where(p => p.Nombre.Trim().ToUpper() + " " + p.Appaterno.Trim().ToUpper() + " " + p.Apmaterno.Trim().ToUpper() ==
							personaCLS.NombreCompleto.Trim().ToUpper() && p.Bhabilitado==1).Count() :
							bDHospitalContext.Persona
							.Where(p => p.Nombre.Trim().ToUpper() + " " + p.Appaterno.Trim().ToUpper() + " " + p.Apmaterno.Trim().ToUpper() ==
							personaCLS.NombreCompleto.Trim().ToUpper() &&
							p.Iidpersona != personaCLS.IdPersona && p.Bhabilitado == 1).Count();

					int nvecesEmail = (personaCLS.IdPersona == 0) ? bDHospitalContext.Persona
							.Where(p => p.Email.Trim().ToUpper() ==	personaCLS.Email.Trim().ToUpper() && p.Bhabilitado == 1).Count() :
							bDHospitalContext.Persona
							.Where(p => p.Email.Trim().ToUpper() == personaCLS.Email.Trim().ToUpper() &&
							p.Iidpersona != personaCLS.IdPersona && p.Bhabilitado == 1).Count();

					if (!ModelState.IsValid || nvecesNombre > 0 || nvecesEmail > 0)
					{
						if (nvecesNombre > 0) personaCLS.MensajeErrorNombre = "El nombre ya está repetido";
						if (nvecesEmail > 0) personaCLS.MensajeErrorEmail = "El email ya está repetido";
						return View(vista,personaCLS);
					}
					else
					{
						Persona persona = (personaCLS.IdPersona != 0) ?
							bDHospitalContext.Persona
								.Where(p => p.Iidpersona == personaCLS.IdPersona).First() : new Persona();

						if (personaCLS.IdPersona != 0)
							persona.Iidpersona = personaCLS.IdPersona;
						persona.Nombre = personaCLS.Nombre;
						persona.Appaterno = personaCLS.Appaterno;
						persona.Apmaterno = personaCLS.Apmaterno;
						persona.Telefonofijo = personaCLS.Telefonofijo;
						persona.Telefonocelular = personaCLS.Telefonocelular;
						persona.Fechanacimiento = personaCLS.Fechanacimiento;
						persona.Email = personaCLS.Email;
						persona.Iidsexo = personaCLS.IdSexo;
						persona.Bdoctor = 0;
						persona.Bpaciente = 0;
						persona.Bhabilitado = 1;
						persona.Foto = personaCLS.Foto;

						if (personaCLS.IdPersona == 0)
							bDHospitalContext.Persona.Add(persona);
						bDHospitalContext.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				return View(vista,personaCLS);
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Eliminar(int idPersona)
		{
			string error;
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					Persona persona = bDHospitalContext.Persona.Where(p => p.Iidpersona == idPersona).First();
					persona.Bhabilitado = 0;
					bDHospitalContext.SaveChanges();
				}
			}
			catch (Exception e)
			{
				error = e.Message;
			}
			return RedirectToAction("Index");
		}


		public PersonaCLS Editar(int id)
		{
			PersonaCLS personaCLS = new PersonaCLS();
			LlenarSexo();

			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				personaCLS = bDHospitalContext.Persona
								.Where(p => p.Iidpersona == id)
								.Select(p => new PersonaCLS
								{
									IdPersona = p.Iidpersona,
									Nombre = p.Nombre,
									Appaterno = p.Appaterno,
									Apmaterno = p.Apmaterno,
									Telefonofijo = p.Telefonofijo,
									Telefonocelular = p.Telefonocelular,
									FechanacimientoCadenaSet = ((DateTime) p.Fechanacimiento).ToString("dd/MM/yyyy"),
									Email = p.Email,
									IdSexo = p.Iidsexo,
									Foto = p.Foto
								}
								)
								.First();
			}

			return personaCLS;
		}



		//public IActionResult Editar(int id)
		//{
		//	PersonaCLS personaCLS = new PersonaCLS();
		//	LlenarSexo();

		//	using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
		//	{
		//		personaCLS = bDHospitalContext.Persona
		//						.Where(p => p.Iidpersona == id)
		//						.Select(p => new PersonaCLS
		//						{
		//							IdPersona = p.Iidpersona,
		//							Nombre = p.Nombre,
		//							Appaterno = p.Appaterno,
		//							Apmaterno = p.Apmaterno,
		//							Telefonofijo = p.Telefonofijo,
		//							Telefonocelular = p.Telefonocelular,
		//							Fechanacimiento = p.Fechanacimiento,
		//							Email = p.Email,
		//							IdSexo = p.Iidsexo
		//	}
		//						)
		//						.First();
		//	}

		//	return View(personaCLS);
		//}
	}
}
