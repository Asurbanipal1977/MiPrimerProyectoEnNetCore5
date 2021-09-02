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

	public class PaginaTipoUsuarioController : BaseController
	{
		public static List<PaginaTipoUsuarioCLS> lista;

		public IActionResult Index()
		{
			LlenarTipoUsuario();
			return View();
		}

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
				lista.Insert(0, new SelectListItem { Value = "", Text = "--Seleccione tipo usuario--" });
			}
			ViewBag.listaTipoUsuario = lista;
		}

		public List<PaginaTipoUsuarioCLS> Listar(int? idTipoUsuario)
		{
			List<PaginaTipoUsuarioCLS> listaTU = new List<PaginaTipoUsuarioCLS>();
			using (BDHospitalContext bd = new BDHospitalContext())
			{
				listaTU = (from tipoUsuarioPagina in bd.TipoUsuarioPagina
						 join tipoUsuario in bd.TipoUsuario on tipoUsuarioPagina.Iidtipousuario equals tipoUsuario.Iidtipousuario
						 join pagina in bd.Pagina on tipoUsuarioPagina.Iidpagina equals pagina.Iidpagina
						 where tipoUsuarioPagina.Bhabilitado == 1 &&
						 (idTipoUsuario==null) ?
						 tipoUsuarioPagina.Iidtipousuario == tipoUsuarioPagina.Iidtipousuario :
						 tipoUsuarioPagina.Iidtipousuario == idTipoUsuario
						 select new PaginaTipoUsuarioCLS
						 {
							 idPaginaTipoUsuario = tipoUsuarioPagina.Iidtipousuariopagina,
							 nombrePagina = pagina.Mensaje,
							 nombreTipoUsuario = tipoUsuario.Nombre
						 }).ToList();
			}

			lista = listaTU;

			return listaTU;
		}


		public List<BotonesCLS> ListarBotones()
		{
			List<BotonesCLS> listPagina = new List<BotonesCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listPagina = bdContext.Boton.Where(b => b.Bhabilitado == 1)
					.Select(b => new BotonesCLS
					{
						idBoton = b.Iidboton,
						nombreBoton = b.Nombre
					}).ToList();
			}

			return listPagina;
		}



		public List<BotonesCLS> RecuperarBotones(int id)
		{
			List<BotonesCLS> listPagina = new List<BotonesCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listPagina = bdContext.TipoUsuarioPaginaBoton.Where(b => b.Bhabilitado == 1 && b.Iidtipousuariopagina==id)
					.Select(b => new BotonesCLS
					{
						idBoton = (int) b.Iidboton
					}).ToList();
			}

			return listPagina;
		}


		public string GuardarDatos(int idPaginaTipoUsuario, int[] aBotones)
		{
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
						using (var dbContextTransaction = bDHospitalContext.Database.BeginTransaction())
						{
							List<TipoUsuarioPaginaBoton> listTipoUsuarioPaginaBoton = bDHospitalContext.TipoUsuarioPaginaBoton
								.Where(e => e.Iidtipousuariopagina == idPaginaTipoUsuario && e.Bhabilitado == 1).ToList();
							foreach (TipoUsuarioPaginaBoton tup in listTipoUsuarioPaginaBoton)
							{
								tup.Bhabilitado = 0;
							}

							TipoUsuarioPaginaBoton tipoUsuarioPaginaBoton;
							//Solo va a enviarse en el editar
							foreach (int elem in aBotones)
							{

								tipoUsuarioPaginaBoton = bDHospitalContext.TipoUsuarioPaginaBoton
								.Where(e => e.Iidtipousuariopagina == idPaginaTipoUsuario && e.Iidboton == elem).FirstOrDefault();

								if (tipoUsuarioPaginaBoton != null)
								{
									tipoUsuarioPaginaBoton.Bhabilitado = 1;
								}
								else
								{
									tipoUsuarioPaginaBoton = new TipoUsuarioPaginaBoton();
									tipoUsuarioPaginaBoton.Iidtipousuariopagina = idPaginaTipoUsuario;
									tipoUsuarioPaginaBoton.Iidboton = elem;
									tipoUsuarioPaginaBoton.Bhabilitado = 1;
									bDHospitalContext.TipoUsuarioPaginaBoton.Add(tipoUsuarioPaginaBoton);
								}
							}

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
	}
}
