using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiPrimerProyectoEnNetCore5.Clases;
using MiPrimerProyectoEnNetCore5.Models;

namespace MiPrimerProyectoEnNetCore5.Controllers
{
	public class PaginaController : BaseController
	{
		public static List<PaginaCLS> lista;

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


		public List<PaginaCLS> Listar(PaginaCLS paginaCLS)
		{
			List<PaginaCLS> listPagina = new List<PaginaCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listPagina = bdContext.Pagina.Where(pagina => pagina.Bhabilitado == 1 &&
				string.IsNullOrWhiteSpace(paginaCLS.Mensaje) ? pagina.Mensaje == pagina.Mensaje : pagina.Mensaje.Contains(paginaCLS.Mensaje))
					.Select(p => new PaginaCLS
					{
						IdPagina = p.Iidpagina,
						Accion = p.Accion,
						Mensaje = p.Mensaje,
						Controlador = p.Controlador
					}).ToList();
				lista = listPagina;
			}

			return listPagina;
		}

		public IActionResult Index(PaginaCLS paginaCLS)
		{
			//List<PaginaCLS> listPagina = new List<PaginaCLS>();
			//using (BDHospitalContext bdContext = new BDHospitalContext())
			//{
			//	listPagina = bdContext.Pagina.Where(pagina => pagina.Bhabilitado == 1 &&
			//	string.IsNullOrWhiteSpace(paginaCLS.Mensaje) ? pagina.Mensaje == pagina.Mensaje : pagina.Mensaje.Contains(paginaCLS.Mensaje))
			//		.Select(p => new PaginaCLS
			//		{
			//			IdPagina = p.Iidpagina,
			//			Accion = p.Accion,
			//			Mensaje = p.Mensaje,
			//			Controlador = p.Controlador
			//		}).ToList();
			//	ViewBag.Mensaje = paginaCLS.Mensaje;
			//}

			//lista = listPagina;
			//return View(listPagina);

			return View();
		}

		public IActionResult Agregar()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Guardar(PaginaCLS paginaCLS)
		{
			string vista = (paginaCLS.IdPagina == 0) ? "Agregar" : "Editar";
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					int nveces = (paginaCLS.IdPagina == 0) ? bDHospitalContext.Pagina
							.Where(e => e.Mensaje.Trim().ToUpper().Equals(paginaCLS.Mensaje.Trim().ToUpper())).Count() :
							bDHospitalContext.Pagina
							.Where(e => e.Mensaje.Trim().ToUpper().Equals(paginaCLS.Mensaje.Trim().ToUpper()) &&
							e.Iidpagina != paginaCLS.IdPagina).Count();
					if (!ModelState.IsValid || nveces > 0)
					{
						if (nveces > 0) paginaCLS.MensajeError = "El mensaje ya está repetido";
						return View(vista,paginaCLS);
					}
					else
					{
						Pagina pagina = (paginaCLS.IdPagina != 0) ?
							bDHospitalContext.Pagina
								.Where(p => p.Iidpagina == paginaCLS.IdPagina).First() : new Pagina();
						
						if (paginaCLS.IdPagina != 0)
							pagina.Iidpagina = paginaCLS.IdPagina;
						pagina.Mensaje = paginaCLS.Mensaje;
						pagina.Accion = paginaCLS.Accion;
						pagina.Controlador = paginaCLS.Controlador;
						pagina.Bhabilitado = 1;

						if (paginaCLS.IdPagina == 0)
							bDHospitalContext.Pagina.Add(pagina);
						bDHospitalContext.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				return View(vista,paginaCLS);
			}

			return RedirectToAction("Index");
		}

		public int Eliminar(int idPagina)
		{
			int resultado = 0;
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					Pagina pagina = bDHospitalContext.Pagina.Where(t => t.Iidpagina == idPagina).First();
					bDHospitalContext.Pagina.Remove(pagina);
					bDHospitalContext.SaveChanges();
					resultado = 1;
				}
			}
			catch (Exception ex)
			{
				
			}


			return resultado;
		}

		public IActionResult Editar(int id)
		{

			PaginaCLS paginaCLS = new PaginaCLS();

			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				paginaCLS = bDHospitalContext.Pagina
								.Where(p => p.Iidpagina == id)
								.Select(p => new PaginaCLS
								{
									IdPagina = p.Iidpagina,
									Mensaje = p.Mensaje,
									Controlador = p.Controlador,
									Accion = p.Accion
								}
								)
								.First();
			}

			return View(paginaCLS);
		}
	}
}
