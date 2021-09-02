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
	public class PacienteController : BaseController
	{
		public static List<PacienteCLS> lista;

		public void LlenarTipoSangre()
		{
			List<SelectListItem> lista = new List<SelectListItem>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				lista = bdContext.TipoSangre.Where(s => s.Bhabilitado == 1)
				.Select(s => new SelectListItem
				{
					Value = s.Iidtiposangre.ToString(),
					Text = s.Nombre
				}).ToList();
				lista.Insert(0, new SelectListItem { Value = "", Text = "--Seleccione tipo de sangre--" });
			}
			ViewBag.listaTipoSangre = lista;
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

			LlenarTipoSangre();
			return View();
		}


		public List<PacienteCLS> Listar(PacienteCLS PacienteCLS)
		{
			List<PacienteCLS> listPaciente = new List<PacienteCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{

				listPaciente = (from paciente in bdContext.Paciente
							  join persona in bdContext.Persona on paciente.Iidpersona equals persona.Iidpersona
							  
							  join tipoSangre in bdContext.TipoSangre on paciente.Iidtiposangre equals tipoSangre.Iidtiposangre
							  where paciente.Bhabilitado == 1 &&
							  (PacienteCLS.nombreCompleto == null ? paciente.Iidpaciente == paciente.Iidpaciente :
							  (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Contains(PacienteCLS.nombreCompleto))
							  select new PacienteCLS
							  {
								  idPaciente = paciente.Iidpaciente,
								  nombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
								  tipoSangre = tipoSangre.Nombre,
								  alergia = paciente.Alergias
							  }
						).ToList();

				lista = listPaciente;
			}

			return listPaciente;
		}


		public string GuardarDatos(PacienteCLS pacienteCLS)
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
							Paciente paciente = (pacienteCLS.idPaciente != 0) ?
								bDHospitalContext.Paciente
									.Where(p => p.Iidpaciente == pacienteCLS.idPaciente).First() : new Paciente();

							if (pacienteCLS.idPaciente != 0)
							{
								paciente.Iidpaciente = pacienteCLS.idPaciente;
							}

							paciente.Iidpersona = pacienteCLS.idPersona;
							paciente.Iidtiposangre = pacienteCLS.idTipoSangre;
							paciente.Alergias = pacienteCLS.alergia;
							paciente.Antecedentesquirurgicos = pacienteCLS.antecedentesQuirurgicos;
							paciente.Cuadrovacunas = pacienteCLS.cuadroVacunas;
							paciente.Enfermedadescronicas = pacienteCLS.enfermedadesCronicas;
							paciente.Bhabilitado = 1;
							if (pacienteCLS.idPaciente == 0)
							{
								bDHospitalContext.Paciente.Add(paciente);
							}

							Persona persona2 = bDHospitalContext.Persona
								.Where(p => p.Iidpersona == pacienteCLS.idPersona).First();
							persona2.Bpaciente = 1;

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


		public PacienteCLS Editar(int id)
		{
			PacienteCLS pacienteCLS = new PacienteCLS();
			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				pacienteCLS = (from paciente in bDHospitalContext.Paciente
							 join persona in bDHospitalContext.Persona on paciente.Iidpersona equals persona.Iidpersona
							 where paciente.Bhabilitado == 1 && paciente.Iidpaciente == id
							 select new PacienteCLS
							 {
								 idPaciente = paciente.Iidpaciente,
								 idPersona = paciente.Iidpersona,
								 nombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
								 idTipoSangre = paciente.Iidtiposangre,
								alergia = paciente.Alergias,
								antecedentesQuirurgicos = paciente.Antecedentesquirurgicos,
								cuadroVacunas = paciente.Cuadrovacunas,
								enfermedadesCronicas= paciente.Enfermedadescronicas
							}).First();
			}

			return pacienteCLS;
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
						Paciente paciente = bDHospitalContext.Paciente.Where(t => t.Iidpaciente == id).First();
						paciente.Bhabilitado = 0;

						Persona persona = bDHospitalContext.Persona
								.Where(p => p.Iidpersona == paciente.Iidpersona).First();
						persona.Bpaciente = 0;

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
