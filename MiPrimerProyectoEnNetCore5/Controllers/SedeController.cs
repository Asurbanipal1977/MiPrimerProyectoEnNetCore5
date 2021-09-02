using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MiPrimerProyectoEnNetCore5.Clases;
using MiPrimerProyectoEnNetCore5.Models;

namespace MiPrimerProyectoEnNetCore5.Controllers
{
	public class SedeController : BaseController
	{
		public FileResult exportar(string[] nombrePropiedades, string tipoReporte)
		{
			switch (tipoReporte)
			{
				case "Excel":
					byte[] buffer = exportarExcelDatos<SedeCLS>(nombrePropiedades, lista);
					return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
				case "Word":
					byte[] bufferWord = exportarWordDatos<SedeCLS>(nombrePropiedades, lista);
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


		public static List<SedeCLS> lista;
		public IActionResult Index(SedeCLS sedeCLS)
		{
			string controlador = ControllerContext.ActionDescriptor.ControllerName;
			List<PaginaCLS> listaBotones = Generic.ListarPaginasControlador(controlador);
			ViewBag.ListaBotones = listaBotones.Select(p => p.IdBoton).ToList();

			List<SedeCLS> listSede = new List<SedeCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				if (string.IsNullOrWhiteSpace(sedeCLS.Nombre))
				{
					listSede = bdContext.Sede.Where(sede => sede.Bhabilitado == 1)
					.Select(sede => new SedeCLS
					{
						Nombre = sede.Nombre,
						idSede = sede.Iidsede,
						Direccion = sede.Direccion
					}).ToList();
					ViewBag.nombreSede = "";
				}
				else
				{
					listSede = bdContext.Sede.Where(sede => sede.Bhabilitado == 1 && sede.Nombre.Contains(sedeCLS.Nombre))
					.Select(sede => new SedeCLS
					{
						Nombre = sede.Nombre,
						idSede = sede.Iidsede,
						Direccion = sede.Direccion
					}).ToList();
					ViewBag.nombreSede = sedeCLS.Nombre;
				}
			}

			lista = listSede;
			return View(listSede);
		}


		public List<SedeCLS> Listar(SedeCLS sedeCLS)
		{
			List<SedeCLS> listSede = new List<SedeCLS>();

			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listSede = bdContext.Sede.Where(sede => sede.Bhabilitado == 1 &&
				((string.IsNullOrWhiteSpace(sedeCLS.Nombre)) ? sede.Iidsede == sede.Iidsede : sede.Nombre.Contains(sedeCLS.Nombre))
				)
					.Select(sede => new SedeCLS
					{
						Nombre = sede.Nombre,
						idSede = sede.Iidsede,
						Direccion = sede.Direccion
					}).ToList();
			}

			return listSede;
		}

		public IActionResult Eliminar(int idSede)
		{
			string error;
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					Sede sede = bDHospitalContext.Sede.Where(e => e.Iidsede == idSede).First();
					sede.Bhabilitado = 0;
					bDHospitalContext.SaveChanges();
				}
			}
			catch (Exception e)
			{
				error = e.Message;
			}
			return RedirectToAction("Index");
		}


		public string GuardarDatos(SedeCLS sedeCLS)
		{
			string vista = (sedeCLS.idSede == 0) ? "Agregar" : "Editar";
			string resultado = "";
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
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
						Sede sede = (sedeCLS.idSede != 0) ?
						bDHospitalContext.Sede
							.Where(e => e.Iidsede == sedeCLS.idSede).First() : new Sede();

						if (sedeCLS.idSede != 0)
							sede.Iidsede = sedeCLS.idSede;
						sede.Nombre = sedeCLS.Nombre;
						sede.Direccion = sedeCLS.Direccion;
						sede.Bhabilitado = 1;

						if (sedeCLS.idSede == 0)
							bDHospitalContext.Sede.Add(sede);

						bDHospitalContext.SaveChanges();
						resultado = "OK";
					}
				}
			}
			catch (Exception ex)
			{
				resultado = "KO";
			}

			return resultado;
		}


		public IActionResult Guardar(SedeCLS sedeCLS)
		{
			string vista = (sedeCLS.idSede == 0) ? "Agregar" : "Editar";
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					if (!ModelState.IsValid)
					{
						return View(vista, sedeCLS);
					}
					else
					{
						Sede sede = (sedeCLS.idSede != 0) ?
							bDHospitalContext.Sede
								.Where(e => e.Iidsede == sedeCLS.idSede).First() : new Sede();

						if (sedeCLS.idSede != 0)
							sede.Iidsede = sedeCLS.idSede;
						sede.Nombre = sedeCLS.Nombre;
						sede.Direccion = sedeCLS.Direccion;
						sede.Bhabilitado = 1;

						if (sedeCLS.idSede == 0)
							bDHospitalContext.Sede.Add(sede);

						bDHospitalContext.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				return View(vista, sedeCLS);
			}

			return RedirectToAction("Index");
		}

		public IActionResult Editar(int id)
		{
			SedeCLS sedeCLS = new SedeCLS();

			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				sedeCLS = bDHospitalContext.Sede
						.Where(s => s.Iidsede == id).Select(s => new SedeCLS
						{
							idSede = s.Iidsede,
							Nombre = s.Nombre,
							Direccion = s.Direccion
						}).First();
			}

			return View(sedeCLS);
		}
	}
}
