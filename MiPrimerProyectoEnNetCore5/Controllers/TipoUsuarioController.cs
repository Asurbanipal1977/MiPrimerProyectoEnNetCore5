using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiPrimerProyectoEnNetCore5.Clases;
using MiPrimerProyectoEnNetCore5.Models;

namespace MiPrimerProyectoEnNetCore5.Controllers
{
	public class TipoUsuarioController : BaseController
	{




		public IActionResult Index(TipoUsuarioCLS tipoUsuarioCLS)
		{
			List<TipoUsuarioCLS> listTipoUsu = new List<TipoUsuarioCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{

				listTipoUsu = bdContext.TipoUsuario.Where(t => t.Bhabilitado == 1
				&& (tipoUsuarioCLS.IdUsuario == 0 ? t.Iidtipousuario == t.Iidtipousuario : t.Iidtipousuario == tipoUsuarioCLS.IdUsuario) 
				&& (string.IsNullOrWhiteSpace(tipoUsuarioCLS.Nombre) ? t.Nombre == t.Nombre : t.Nombre.Contains(tipoUsuarioCLS.Nombre))
				&& (string.IsNullOrWhiteSpace(tipoUsuarioCLS.Descripcion) ? t.Descripcion == t.Descripcion : t.Descripcion.Contains(tipoUsuarioCLS.Descripcion))
				).Select(t => new TipoUsuarioCLS
					{
						IdUsuario = t.Iidtipousuario,
						Nombre = t.Nombre,
						Descripcion = t.Descripcion
					}).ToList();

				ViewBag.IdTipoUsuario = tipoUsuarioCLS.IdUsuario;
				ViewBag.Nombre = tipoUsuarioCLS.Nombre;
				ViewBag.Descripcion = tipoUsuarioCLS.Descripcion;
			}
			return View(listTipoUsu);
		}

		public IActionResult Agregar()
		{
			return View();
		}


		public TipoUsuarioCLS Editar(int id)
		{
			TipoUsuarioCLS tipoUsuarioCLS = new TipoUsuarioCLS();
			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				tipoUsuarioCLS = bDHospitalContext.TipoUsuario
								.Where(p => p.Iidtipousuario == id)
								.Select(p => new TipoUsuarioCLS
									{
										IdUsuario = p.Iidtipousuario,
										Nombre = p.Nombre,
										Descripcion = p.Descripcion,
										listaPaginas = RecuperarPaginas(id)
								}
								)
								.First();
			}
			return tipoUsuarioCLS;
		}

		public List<PaginaCLS> RecuperarPaginas(int id)
		{
			List<PaginaCLS> lista = new List<PaginaCLS>();
			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				lista = bDHospitalContext.TipoUsuarioPagina
						.Where(p => p.Iidtipousuario == id && p.Bhabilitado == 1)
						.Select(p => new PaginaCLS
						{
							IdPagina = (int)p.Iidpagina,
							IdVista = (int)(p.Iidvista == null ? 0 : p.Iidvista)
						}
						)
						.ToList();
			}
			return lista;
		}


		[HttpPost]
		public string GuardarDatos(TipoUsuarioCLS tipoUsuarioCLS, int[] aPaginas, int[] aVistas)
		{
			string vista = (tipoUsuarioCLS.IdUsuario == 0) ? "Agregar" : "Editar";
			string resultado = "";
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					if (tipoUsuarioCLS.Nombre == null) tipoUsuarioCLS.Nombre = "";
					if (tipoUsuarioCLS.Descripcion == null) tipoUsuarioCLS.Descripcion = "";
					int nvecesNombre = (tipoUsuarioCLS.IdUsuario == 0) ? bDHospitalContext.TipoUsuario
							.Where(t => t.Nombre.Trim().ToUpper().Equals(tipoUsuarioCLS.Nombre.Trim().ToUpper())).Count() :
							bDHospitalContext.TipoUsuario
							.Where(t => t.Nombre.Trim().ToUpper().Equals(tipoUsuarioCLS.Nombre.Trim().ToUpper()) &&
							t.Iidtipousuario != tipoUsuarioCLS.IdUsuario).Count();
					if (!ModelState.IsValid || nvecesNombre > 0)
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

						resultado += "</ul>";
					}
					else
					{
						using (var dbContextTransaction = bDHospitalContext.Database.BeginTransaction())
						{
							TipoUsuario tipoUsuario = (tipoUsuarioCLS.IdUsuario != 0) ?
							bDHospitalContext.TipoUsuario
								.Where(e => e.Iidtipousuario == tipoUsuarioCLS.IdUsuario).First() : new TipoUsuario();

							if (tipoUsuarioCLS.IdUsuario != 0)
								tipoUsuario.Iidtipousuario = tipoUsuarioCLS.IdUsuario;
							tipoUsuario.Nombre = tipoUsuarioCLS.Nombre;
							tipoUsuario.Descripcion = tipoUsuarioCLS.Descripcion;
							tipoUsuario.Bhabilitado = 1;

							if (tipoUsuarioCLS.IdUsuario == 0)
								bDHospitalContext.TipoUsuario.Add(tipoUsuario);


							List<TipoUsuarioPagina> listTipoUsuarioPagina = bDHospitalContext.TipoUsuarioPagina
								.Where(e => e.Iidtipousuario == tipoUsuarioCLS.IdUsuario && e.Bhabilitado==1).ToList();
							foreach (TipoUsuarioPagina tup in listTipoUsuarioPagina)
							{
								tup.Bhabilitado = 0;
							}

							TipoUsuarioPagina tipoUsuarioPagina;

							int indice = 0;
							//Solo va a enviarse en el editar
							foreach (int elem in aPaginas)
							{

								tipoUsuarioPagina = bDHospitalContext.TipoUsuarioPagina
								.Where(e => e.Iidtipousuario == tipoUsuarioCLS.IdUsuario && e.Iidpagina == elem).FirstOrDefault();

								if (tipoUsuarioPagina!=null)
								{
									tipoUsuarioPagina.Bhabilitado = 1;
									tipoUsuarioPagina.Iidvista = aVistas[indice++];
								}
								else
								{
									tipoUsuarioPagina = new TipoUsuarioPagina();
									tipoUsuarioPagina.Iidtipousuario = tipoUsuario.Iidtipousuario;
									tipoUsuarioPagina.Iidpagina = elem;
									tipoUsuarioPagina.Bhabilitado = 1;
									tipoUsuarioPagina.Iidvista = aVistas[indice++];
									bDHospitalContext.TipoUsuarioPagina.Add(tipoUsuarioPagina);
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

		[HttpPost]
		public IActionResult Guardar(TipoUsuarioCLS tipoUsuarioCLS)
		{
			string vista = (tipoUsuarioCLS.IdUsuario == 0) ? "Agregar" : "Editar";
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					if (tipoUsuarioCLS.Nombre == null) tipoUsuarioCLS.Nombre = "";
					if (tipoUsuarioCLS.Descripcion == null) tipoUsuarioCLS.Descripcion = "";
					int nvecesNombre = (tipoUsuarioCLS.IdUsuario == 0) ? bDHospitalContext.TipoUsuario
							.Where(t => t.Nombre.Trim().ToUpper().Equals(tipoUsuarioCLS.Nombre.Trim().ToUpper())).Count() :
							bDHospitalContext.TipoUsuario
							.Where(t => t.Nombre.Trim().ToUpper().Equals(tipoUsuarioCLS.Nombre.Trim().ToUpper()) &&
							t.Iidtipousuario != tipoUsuarioCLS.IdUsuario).Count();
					int nvecesDescripcion = (tipoUsuarioCLS.IdUsuario == 0) ? bDHospitalContext.TipoUsuario
							.Where(t => t.Descripcion.Trim().ToUpper().Equals(tipoUsuarioCLS.Descripcion.Trim().ToUpper())).Count() :
							bDHospitalContext.TipoUsuario
							.Where(t => t.Descripcion.Trim().ToUpper().Equals(tipoUsuarioCLS.Descripcion.Trim().ToUpper()) &&
							t.Iidtipousuario != tipoUsuarioCLS.IdUsuario).Count();

					if (!ModelState.IsValid || nvecesNombre > 0 || nvecesDescripcion > 0)
					{
						if (nvecesNombre > 0) tipoUsuarioCLS.MensajeErrorNombre = "El nombre ya está repetido";
						if (nvecesDescripcion > 0) tipoUsuarioCLS.MensajeErrorDescripcion = "La descripción ya está repetida";
						return View(vista,tipoUsuarioCLS);
					}
					else
					{
						TipoUsuario tipoUsuario = new TipoUsuario();
						tipoUsuario.Nombre = tipoUsuarioCLS.Nombre;
						tipoUsuario.Descripcion = tipoUsuarioCLS.Descripcion;
						tipoUsuario.Bhabilitado = 1;
						bDHospitalContext.TipoUsuario.Add(tipoUsuario);

						bDHospitalContext.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				return View(vista,tipoUsuarioCLS);
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult Eliminar(int idTipoUsuario)
		{
			string error;
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					TipoUsuario tipoUsuario = bDHospitalContext.TipoUsuario.Where(t=>t.Iidtipousuario==idTipoUsuario).First();
					bDHospitalContext.TipoUsuario.Remove(tipoUsuario);
					bDHospitalContext.SaveChanges();

				}
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}


			return RedirectToAction("Index");
		}
	}
}
