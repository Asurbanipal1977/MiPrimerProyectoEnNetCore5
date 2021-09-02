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
	public class CitaController : BaseController
	{
		public IActionResult Index()
		{
			LlenarEstadoCita();
			int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));
			int idVista = Generic.ObtenerTipoVista("Cita", idUsuario);

			ViewBag.idVista = idVista;
			return View();
		}

		public void LlenarEstadoCita()
		{
			List<SelectListItem> list = new List<SelectListItem>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				list = bdContext.EstadoCita.Where(s => s.Bhabilitado == 1)
				.Select(s => new SelectListItem
				{
					Value = s.Iidestado.ToString(),
					Text = s.Vnombre
				}).ToList();
				list.Insert(0, new SelectListItem { Value = "", Text = "--Seleccione estado--" });
			}
			ViewBag.listEstadoCita = list;
		}

		public string GuardarDatos(CitaCLS citaCLS)
		{
			string resultado = "";
			int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));

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
							Cita cita = (citaCLS.idCita != 0) ?
								bDHospitalContext.Cita
									.Where(p => p.Iidcita == citaCLS.idCita).First() : new Cita();

							HistorialCita hc = new HistorialCita();

							if (citaCLS.idCita != 0)
							{
								cita.Iidcita = citaCLS.idCita;
							}

							cita.Iidpersona = citaCLS.idPersona;
							cita.Iidsede = citaCLS.idSede;
							cita.Dfechainicioenfermedad = citaCLS.fechaEnfermedad;
							cita.Descripcionenfermedad = citaCLS.descripcionEnfermedad;
							cita.Iidusuario = idUsuario;
							cita.Iidestadocita = 1;
							cita.Bhabilitado = 1;
							if (citaCLS.idCita == 0)
							{
								bDHospitalContext.Cita.Add(cita);
							}
							bDHospitalContext.SaveChanges();

							int idCita = cita.Iidcita;

							hc.Iidcita = idCita;
							hc.Dfecha = DateTime.Now;
							hc.Iidestado = 1;
							hc.Iidusuario = idUsuario;
							bDHospitalContext.HistorialCita.Add(hc);

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

		public List<HistorialCitaCLS> ListarHistorial(int idCita)
		{
			List<HistorialCitaCLS> listCita = new List<HistorialCitaCLS>();

			using (BDHospitalContext bd = new BDHospitalContext())
			{

				listCita = (from h in bd.HistorialCita
							join c in bd.Cita on h.Iidcita equals c.Iidcita
							join u in bd.Usuario on h.Iidusuario equals u.Iidusuario
							join p in bd.Persona on u.Iidpersona equals p.Iidpersona
							join e in bd.EstadoCita on h.Iidestado equals e.Iidestado
							where h.Iidcita == idCita && c.Bhabilitado==1
							select new HistorialCitaCLS
							{
								idHistorialCita = (int)h.Iidhistorialcita,
								idCita = (int) c.Iidcita,
								fechaEstado = (h.Dfecha == null ? "" : h.Dfecha.Value.ToShortDateString()),
								nombreUsuario = p.Nombre + " " + p.Appaterno + " " + p.Apmaterno,
								estadoCita = e.Vnombre
							}
						).ToList();

			}

			return listCita;
		}

		public List<CitaCLS> Listar(int? idEstado)
		{
			List<CitaCLS> listCita = new List<CitaCLS>();
			int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));
			int idVista = Generic.ObtenerTipoVista("Cita", idUsuario);

			using (BDHospitalContext bd = new BDHospitalContext())
			{

				listCita = (from c in bd.Cita
							join p in bd.Persona on c.Iidpersona equals p.Iidpersona
							join u in bd.Usuario on c.Iidusuario equals u.Iidusuario
							join e in bd.EstadoCita on c.Iidestadocita equals e.Iidestado
							where c.Bhabilitado==1 &&
							(idEstado != null ? c.Iidestadocita == idEstado : c.Iidcita == c.Iidcita) &&
							(idVista == 1 ? c.Iidcita == c.Iidcita : (c.Iidusuario == idUsuario))
							select new CitaCLS
							  {
								  idCita = c.Iidcita,
								  nombreCompleto = p.Nombre+" "+ p.Appaterno+" "+p.Apmaterno,
								  fechaEnfermedadCadena = (c.Dfechainicioenfermedad==null?"": c.Dfechainicioenfermedad.Value.ToShortDateString()),
								  nombreUsuario = u.Nombreusuario,
								  estadoCita = e.Vnombre,
								  idEstadoCita = e.Iidestado
							  }
						).ToList();

			}

			return listCita;
		}
		public int CambiarEstado(int idCita, int idEstadoACambiar, string motivo="")
		{
			int resultado = 0;
			int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));

			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				using (var dbContextTransaction = bDHospitalContext.Database.BeginTransaction())
				{
					try
					{
						Cita cita = bDHospitalContext.Cita
								.Where(c => c.Iidcita == idCita).First();
						cita.Iidestadocita = idEstadoACambiar;

						HistorialCita hc = new HistorialCita();
						hc.Iidcita = idCita;
						hc.Dfecha = DateTime.Now;
						hc.Iidestado = idEstadoACambiar;
						hc.Iidusuario = idUsuario;
						hc.Vobservacion = motivo;
						bDHospitalContext.HistorialCita.Add(hc);

						bDHospitalContext.SaveChanges();
						dbContextTransaction.Commit();
						resultado = 1;
					}
					catch (Exception e)
					{
						dbContextTransaction.Rollback();
					}
				}
			}

			return resultado;
		}


		public int EliminarCita(int idCita)
		{
			int resultado = 0;
			int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));

			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				using (var dbContextTransaction = bDHospitalContext.Database.BeginTransaction())
				{
					try
					{
						Cita cita = bDHospitalContext.Cita
								.Where(c => c.Iidcita == idCita).First();
						cita.Bhabilitado = 0;

						bDHospitalContext.SaveChanges();
						dbContextTransaction.Commit();
						resultado = 1;
					}
					catch (Exception e)
					{
						dbContextTransaction.Rollback();
					}
				}
			}

			return resultado;
		}
	}
}
