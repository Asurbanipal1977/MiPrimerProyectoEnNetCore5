using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiPrimerProyectoEnNetCore5.Clases;
using MiPrimerProyectoEnNetCore5.Filters;
using MiPrimerProyectoEnNetCore5.Models;
using OfficeOpenXml;
using cm = System.ComponentModel;

namespace MiPrimerProyectoEnNetCore5.Controllers
{
	public class EspecialidadController : BaseController
	{
		public static List<EspecialidadCLS> lista;

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

		public IActionResult Index(EspecialidadCLS especialidad)
		{
			string controlador = ControllerContext.ActionDescriptor.ControllerName;
			List<PaginaCLS> listaBotones = Generic.ListarPaginasControlador(controlador);
			ViewBag.ListaBotones = listaBotones.Select(p=>p.IdBoton).ToList();

			return View();
		}


		public List<EspecialidadCLS> Listar(EspecialidadCLS especialidadCLS)
		{
			List<EspecialidadCLS> listaEspecialidad = new List<EspecialidadCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listaEspecialidad = bdContext.Especialidad.Where(e => e.Bhabilitado == 1 &&
					(string.IsNullOrEmpty(especialidadCLS.nombre) ? e.Iidespecialidad == e.Iidespecialidad : e.Nombre.Contains(especialidadCLS.nombre)))
					.Select(e => new EspecialidadCLS
					{
						idEspecialidad = e.Iidespecialidad,
						nombre = e.Nombre,
						descripcion = e.Descripcion
					}).ToList();
				lista = listaEspecialidad;
			}
			return listaEspecialidad;
		}

		public List<EspecialidadCLS> Filtrar(EspecialidadCLS especialidad)
		{
			List<EspecialidadCLS> listaEspecialidad = new List<EspecialidadCLS>();

			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listaEspecialidad = bdContext.Especialidad.Where(e => e.Bhabilitado == 1 && e.Nombre.Contains(especialidad.nombre))
						.Select(e => new EspecialidadCLS
						{
							idEspecialidad = e.Iidespecialidad,
							nombre = e.Nombre,
							descripcion = e.Descripcion
						}).ToList();
				lista = listaEspecialidad;
				return listaEspecialidad;
			}
		}

		public IActionResult Agregar()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Guardar(EspecialidadCLS especialidadCLS)
		{
			string vista = (especialidadCLS.idEspecialidad == 0) ? "Agregar" : "Editar";
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					int nveces = (especialidadCLS.idEspecialidad == 0) ? bDHospitalContext.Especialidad
							.Where(e => e.Nombre.ToUpper().Equals(especialidadCLS.nombre.ToUpper())).Count() :
							bDHospitalContext.Especialidad
							.Where(e => e.Nombre.ToUpper().Equals(especialidadCLS.nombre.ToUpper()) &&
							e.Iidespecialidad != especialidadCLS.idEspecialidad).Count();
					if (!ModelState.IsValid || nveces > 0)
					{
						if (nveces > 0) especialidadCLS.MensajeError = "El nombre de la especialidad ya está repetido";
						return View(vista, especialidadCLS);
					}
					else
					{
						Especialidad especialidad = new Especialidad();
						
						if (especialidadCLS.idEspecialidad == 0)
						{
							especialidad = new Especialidad();
							especialidad.Nombre = especialidadCLS.nombre;
							especialidad.Descripcion = especialidadCLS.descripcion;
							especialidad.Bhabilitado = 1;
							bDHospitalContext.Especialidad.Add(especialidad);
							bDHospitalContext.SaveChanges();
						}
						else
						{
							especialidad = bDHospitalContext.Especialidad
								.Where(e => e.Iidespecialidad == especialidadCLS.idEspecialidad).First();
							especialidad.Iidespecialidad = especialidadCLS.idEspecialidad;
							especialidad.Nombre = especialidadCLS.nombre;
							especialidad.Descripcion = especialidadCLS.descripcion;
							bDHospitalContext.SaveChanges();
						}
					}
				}
			}
			catch (Exception ex)
			{
				return View(vista, especialidadCLS);
			}

			return RedirectToAction("Index");
		}

		//[HttpPost]
		//public IActionResult Eliminar(string idEspecialidad)
		//{
		//	string error;
		//	try
		//	{
		//		using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
		//		{
		//			Especialidad especialidad = bDHospitalContext.Especialidad.Where(e => e.Iidespecialidad == int.Parse(idEspecialidad)).First();
		//			especialidad.Bhabilitado = 0;
		//			bDHospitalContext.SaveChanges();
		//		}
		//	}
		//	catch (Exception e)
		//	{
		//		error = e.Message;
		//	}
		//	return RedirectToAction("Index");
		//}

		public int Eliminar(string idEspecialidad)
		{
			int respuesta=0;
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					Especialidad especialidad = bDHospitalContext.Especialidad.Where(e => e.Iidespecialidad == int.Parse(idEspecialidad)).First();
					especialidad.Bhabilitado = 0;
					bDHospitalContext.SaveChanges();
					respuesta = 1;
				}
			}
			catch (Exception e){}

			return respuesta;
		}

		public IActionResult Editar(int id)
		{
			EspecialidadCLS especialidadCLS = new EspecialidadCLS();
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					especialidadCLS = bDHospitalContext.Especialidad
						.Where(e => e.Iidespecialidad == id).Select(e => new EspecialidadCLS
						{
							idEspecialidad = e.Iidespecialidad,
							nombre = e.Nombre,
							descripcion = e.Descripcion
						}).First();
				}
			}
			catch (Exception e)
			{				
			}
			return View(especialidadCLS);
		}
	}	
}
