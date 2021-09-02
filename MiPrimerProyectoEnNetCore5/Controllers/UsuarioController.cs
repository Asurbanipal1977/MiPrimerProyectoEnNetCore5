using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiPrimerProyectoEnNetCore5.Clases;
using MiPrimerProyectoEnNetCore5.Models;

namespace MiPrimerProyectoEnNetCore5.Controllers
{
	public class UsuarioController : BaseController
	{
		public static List<UsuarioCLS> lista;


		public void LlenarTipoUsuario()
		{
			List<SelectListItem> lista = new List<SelectListItem>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				lista = bdContext.TipoUsuario.Where(s => s.Bhabilitado == 1)
				.Select(s => new SelectListItem
				{
					Value = s.Iidtipousuario.ToString(),
					Text = s.Nombre
				}).ToList();
				lista.Insert(0, new SelectListItem { Value = "", Text = "--Seleccione tipo de usuario--" });
			}
			ViewBag.listaTipoUsuario = lista;
		}

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


		public IActionResult Index()
		{
			string controlador = ControllerContext.ActionDescriptor.ControllerName;
			List<PaginaCLS> listaBotones = Generic.ListarPaginasControlador(controlador);
			ViewBag.ListaBotones = listaBotones.Select(p => p.IdBoton).ToList();

			LlenarTipoUsuario();
			return View();
		}

		public List<UsuarioCLS> Listar(UsuarioCLS UsuarioCLS)
		{
			List<UsuarioCLS> listUsuario = new List<UsuarioCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{

				listUsuario = (from usuario in bdContext.Usuario
								join persona in bdContext.Persona on usuario.Iidpersona equals persona.Iidpersona
								join tipo in bdContext.TipoUsuario on usuario.Iidtipousuario equals tipo.Iidtipousuario
								where usuario.Bhabilitado == 1 &&
								(UsuarioCLS.nombreUsuario == null ? usuario.Iidpersona == usuario.Iidpersona :
								(usuario.Nombreusuario.Trim().ToUpper()).Contains(UsuarioCLS.nombreUsuario.Trim().ToUpper()))
								select new UsuarioCLS
								{
									idUsuario = usuario.Iidusuario,
									nombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
									nombreUsuario = usuario.Nombreusuario,
									nombreTipoUsuario = tipo.Nombre
								}
						).ToList();

				lista = listUsuario;
			}

			return listUsuario;
		}


		public string GuardarDatos(UsuarioCLS usuarioCLS)
		{
			string resultado = "";

			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				using (var dbContextTransaction = bDHospitalContext.Database.BeginTransaction())
				{
					try
					{
						if (!ModelState.IsValid)
						{
							var listElementos = (from model in ModelState.Values
												 from errors in model.Errors
												 select errors.ErrorMessage).ToList();

							resultado = "<ul class='list-group'>";
							foreach (var item in listElementos)
							{
								resultado += "<li class='list-group-item list-group-item-danger'>" + item + "</li>";
							}

							resultado += "</ul>";
						}
						else
						{
							Usuario usuario = (usuarioCLS.idUsuario != 0) ?
								bDHospitalContext.Usuario
									.Where(p => p.Iidusuario == usuarioCLS.idUsuario).First() : new Usuario();

							if (usuarioCLS.idUsuario != 0)
							{
								usuario.Iidusuario = usuarioCLS.idUsuario;
								Persona persona = bDHospitalContext.Persona
								.Where(p => p.Iidpersona == usuario.Iidpersona).First();
								persona.Btieneusuario = 0;
							}

							usuario.Iidpersona = usuarioCLS.idPersona;
							usuario.Iidtipousuario = (int) usuarioCLS.idTipoUsuario;
							usuario.Nombreusuario = usuarioCLS.nombreUsuario;
							usuario.Bhabilitado = 1;
							if (usuarioCLS.idUsuario == 0)
							{
								usuario.Contraseña = Generic.CifrarDatos(usuarioCLS.contra);
								bDHospitalContext.Usuario.Add(usuario);
							}

							Persona persona2 = bDHospitalContext.Persona
								.Where(p => p.Iidpersona == usuarioCLS.idPersona).First();
							persona2.Btieneusuario = 1;

							bDHospitalContext.SaveChanges();
							dbContextTransaction.Commit();
							resultado = "OK";
						}
					}
					catch (Exception e)
					{
						resultado = e.Message;
						dbContextTransaction.Rollback();
					}
				}
			}

			return resultado;
		}

		public UsuarioCLS Editar(int id)
		{
			UsuarioCLS usuarioCLS = new UsuarioCLS();
			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				usuarioCLS = (from usuario in bDHospitalContext.Usuario
							   join persona in bDHospitalContext.Persona on usuario.Iidpersona equals persona.Iidpersona
							   where usuario.Bhabilitado == 1 && usuario.Iidusuario == id
							   select new UsuarioCLS
							   {
								   idUsuario = usuario.Iidusuario,
								   idPersona = usuario.Iidpersona,
								   nombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
								   idTipoUsuario = usuario.Iidtipousuario,
								   nombreUsuario = usuario.Nombreusuario,
								   contra = usuario.Contraseña,
								   contra2 = usuario.Contraseña
							   }).First();
			}

			return usuarioCLS;
		}

		public int Eliminar(int id)
		{
			int resultado = 0;

			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				using (var dbContextTransaction = bDHospitalContext.Database.BeginTransaction())
				{
					try
					{
						Usuario usuario = bDHospitalContext.Usuario.Where(t => t.Iidusuario == id).First();
						usuario.Bhabilitado = 0;

						Persona persona = bDHospitalContext.Persona
								.Where(p => p.Iidpersona == usuario.Iidpersona).First();
						persona.Btieneusuario = 0;

						bDHospitalContext.SaveChanges();
						resultado = 1;
						dbContextTransaction.Commit();
					}
					catch (Exception ex)
					{
						dbContextTransaction.Rollback();
					}
				}
			}

			return resultado;
		}
	}
}
