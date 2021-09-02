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
	public class AgregarUsuarioController : Controller
	{
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
				listSexo.Insert(0, new SelectListItem { Value = "", Text = "--Seleccione sexo--" });
			}
			ViewBag.listSexo = listSexo;
		}

		public IActionResult Index()
		{
			LlenarSexo();
			return View();
		}

		public string GuardarDatos(PersonaRegistrarCLS personaCLS)
		{
			string resultado = "";
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					using (var dbContextTransaction = bDHospitalContext.Database.BeginTransaction())
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
							Persona persona = new Persona();
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
							persona.Bdoctor = 0;
							persona.Bpaciente = 0;
							persona.Btieneusuario = 1;
							persona.Iidusuario = int.Parse(HttpContext.Session.GetString("usuario")); //Usuario que registra
							bDHospitalContext.Persona.Add(persona);
							

							bDHospitalContext.SaveChanges();
							int idPersona = persona.Iidpersona;

							Usuario usuario = new Usuario();

							usuario.Iidpersona = idPersona;
							usuario.Iidtipousuario = 3;
							usuario.Nombreusuario = personaCLS.nombreUsuario;
							usuario.Bhabilitado = 1;
							usuario.Contraseña = Generic.CifrarDatos(personaCLS.contra);
							bDHospitalContext.Usuario.Add(usuario);
							bDHospitalContext.SaveChanges();
							dbContextTransaction.Commit();

							resultado = "OK";
						}
					}
				}
			}
			catch (Exception ex)
			{
				resultado = ex.Message;
			}

			return resultado;
		}
	}
}
