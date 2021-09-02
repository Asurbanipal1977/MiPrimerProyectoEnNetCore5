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
	public class DoctorController : BaseController
	{
		public static List<DoctorCLS> lista;

		public void LlenarSede()
		{
			List<SelectListItem> lista = new List<SelectListItem>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				lista = bdContext.Sede.Where(s => s.Bhabilitado == 1)
				.Select(s => new SelectListItem
				{
					Value = s.Iidsede.ToString(),
					Text = s.Nombre
				}).ToList();
				lista.Insert(0, new SelectListItem { Value = "", Text = "--Seleccione sede--" });
			}
			ViewBag.listaSede = lista;
		}


		public void LlenarEspecialidad()
		{
			List<SelectListItem> lista = new List<SelectListItem>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				lista = bdContext.Especialidad.Where(s => s.Bhabilitado == 1)
				.Select(s => new SelectListItem
				{
					Value = s.Iidespecialidad.ToString(),
					Text = s.Nombre
				}).ToList();
				lista.Insert(0, new SelectListItem { Value = "", Text = "--Seleccione especialidad--" });
			}
			ViewBag.listaEspecialidad = lista;
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

			LlenarSede();
			LlenarEspecialidad();
			return View();
		}


		public List<DoctorCLS> Listar(DoctorCLS doctorCLS)
		{
			List<DoctorCLS> listDoctor = new List<DoctorCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{

				listDoctor = (from doctor in bdContext.Doctor
							 join persona in bdContext.Persona on doctor.Iidpersona equals persona.Iidpersona
							 join sede in bdContext.Sede on doctor.Iidsede equals sede.Iidsede
							 join especialidad in bdContext.Especialidad on doctor.Iidespecialidad equals especialidad.Iidespecialidad
							  where doctor.Bhabilitado == 1 &&
							  (doctorCLS.nombreCompleto == null ? doctor.Iiddoctor == doctor.Iiddoctor:
							   (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Contains(doctorCLS.nombreCompleto))
							 select new DoctorCLS
							 {
								 idDoctor = doctor.Iiddoctor,
								 nombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
								 nombreSede = sede.Nombre,
								 especialidad = especialidad.Nombre
							}
						).ToList();

				lista = listDoctor;
			}

			return listDoctor;
		}

		public string GuardarDatos(DoctorCLS doctorCLS)
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
							Doctor doctor = (doctorCLS.idDoctor != 0) ?
								bDHospitalContext.Doctor
									.Where(p => p.Iiddoctor == doctorCLS.idDoctor).First() : new Doctor();

							if (doctorCLS.idDoctor != 0)
							{
								doctor.Iiddoctor = doctorCLS.idDoctor;
							}
							
							doctor.Iidpersona = doctorCLS.idPersona;
							doctor.Iidsede = doctorCLS.idSede;
							doctor.Iidespecialidad = doctorCLS.idEspecialidad;
							doctor.Sueldo = doctorCLS.sueldo;
							doctor.Fechacontrato = doctorCLS.fechaContrato;
							doctor.Bhabilitado = 1;
							if (doctorCLS.idDoctor == 0)
							{
								bDHospitalContext.Doctor.Add(doctor);
							}

							Persona persona2 = bDHospitalContext.Persona
								.Where(p => p.Iidpersona == doctorCLS.idPersona).First();
							persona2.Bdoctor = 1;

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

		public DoctorCLS Editar(int id)
		{
			DoctorCLS doctorCLS = new DoctorCLS();
			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				doctorCLS = (from doctor in bDHospitalContext.Doctor
							 join persona in bDHospitalContext.Persona on doctor.Iidpersona equals persona.Iidpersona
							 where doctor.Bhabilitado == 1 && doctor.Iiddoctor == id
							 join sede in bDHospitalContext.Sede on doctor.Iidsede equals sede.Iidsede
							 join especialidad in bDHospitalContext.Especialidad on doctor.Iidespecialidad equals especialidad.Iidespecialidad
							 select new DoctorCLS
							   {
								   idDoctor = doctor.Iiddoctor,
								   idPersona = doctor.Iidpersona,
								   nombreCompleto = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}",
								   nombreSede = sede.Nombre,
								   especialidad= especialidad.Nombre,
								   idSede = doctor.Iidsede,
								   idEspecialidad = doctor.Iidespecialidad,
								   sueldo = doctor.Sueldo,
								   fechaContrato = doctor.Fechacontrato
							 }).First();
			}

			return doctorCLS;
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
						Doctor doctor = bDHospitalContext.Doctor.Where(t => t.Iiddoctor == id).First();
						doctor.Bhabilitado = 0;

						Persona persona = bDHospitalContext.Persona
								.Where(p => p.Iidpersona == doctor.Iidpersona).First();
						persona.Bdoctor = 0;

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
