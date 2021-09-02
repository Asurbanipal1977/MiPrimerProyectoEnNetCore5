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
	public class MedicamentoController : BaseController
	{
		public static List<MedicamentoCLS> lista;

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

		public List<SelectListItem> listarFormaFarmaceutica()
		{
			List<SelectListItem> listFormaFarmaceutica = new List<SelectListItem>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				listFormaFarmaceutica = bdContext.FormaFarmaceutica.Where(ff => ff.Bhabilitado == 1).Select(ff => new SelectListItem
				{
					Value = ff.Iidformafarmaceutica.ToString(),
					Text=ff.Nombre
				}).ToList();
				listFormaFarmaceutica.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
			}
			return listFormaFarmaceutica;
		}

		public List<MedicamentoCLS> Listar(MedicamentoCLS medicamentoCLS)
		{

			List<MedicamentoCLS> listMedicamento = new List<MedicamentoCLS>();
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{

				listMedicamento = bdContext.Medicamento.Where(m => m.Bhabilitado == 1 &&
				(medicamentoCLS.IdFormaFarmaceutica == 0 || medicamentoCLS.IdFormaFarmaceutica == null ? m.Iidformafarmaceutica == m.Iidformafarmaceutica :
				m.Iidformafarmaceutica == medicamentoCLS.IdFormaFarmaceutica)).Join(bdContext.FormaFarmaceutica, m => m.Iidformafarmaceutica, ff => ff.Iidformafarmaceutica,
					(m, ff) => new MedicamentoCLS
					{
						IdMedicamento = m.Iidmedicamento,
						Nombre = m.Nombre,
						Precio = m.Precio,
						Stock = m.Stock,
						NombreForma = ff.Nombre
					}).ToList();
				lista = listMedicamento;
			}

			return listMedicamento;
		}

		public IActionResult Index()
		{
			string controlador = ControllerContext.ActionDescriptor.ControllerName;
			List<PaginaCLS> listaBotones = Generic.ListarPaginasControlador(controlador);
			ViewBag.ListaBotones = listaBotones.Select(p => p.IdBoton).ToList();

			ViewBag.listFormaFarmaceutica = listarFormaFarmaceutica();
			return View();
		}


		//public IActionResult Index(MedicamentoCLS medicamentoCLS)
		//{
		//	List<MedicamentoCLS> listMedicamento = new List<MedicamentoCLS>();
		//	ViewBag.listFormaFarmaceutica = listarFormaFarmaceutica();
		//	using (BDHospitalContext bdContext = new BDHospitalContext())
		//	{

		//			listMedicamento = bdContext.Medicamento.Where(m => m.Bhabilitado == 1 &&
		//			(medicamentoCLS.IdFormaFarmaceutica == 0 || medicamentoCLS.IdFormaFarmaceutica==null ? m.Iidformafarmaceutica == m.Iidformafarmaceutica : 
		//			m.Iidformafarmaceutica==medicamentoCLS.IdFormaFarmaceutica)).Join(bdContext.FormaFarmaceutica, m => m.Iidformafarmaceutica, ff => ff.Iidformafarmaceutica,
		//				(m, ff) => new MedicamentoCLS
		//				{
		//					IdMedicamento = m.Iidmedicamento,
		//					Nombre = m.Nombre,
		//					Precio = m.Precio,
		//					Stock = m.Stock,
		//					NombreForma = ff.Nombre
		//				}).ToList();
		//	}
		//	return View(listMedicamento);
		//}

		public IActionResult Agregar()
		{
			ViewBag.listFormaFarmaceutica = listarFormaFarmaceutica();
			return View();
		}


		[HttpPost]
		public IActionResult Guardar(MedicamentoCLS medicamentoCLS)
		{
			string vista = (medicamentoCLS.IdMedicamento == 0) ? "Agregar" : "Editar";
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					if (!ModelState.IsValid)
					{
						ViewBag.listFormaFarmaceutica = listarFormaFarmaceutica();
						return View(vista,medicamentoCLS);
					}
					else
					{
						Medicamento medicamento = (medicamentoCLS.IdMedicamento != 0) ?
							bDHospitalContext.Medicamento
								.Where(e => e.Iidmedicamento == medicamentoCLS.IdMedicamento).First() : new Medicamento();

						if (medicamentoCLS.IdMedicamento != 0)
							medicamento.Iidmedicamento = medicamentoCLS.IdMedicamento;
						medicamento.Nombre = medicamentoCLS.Nombre;
						medicamento.Concentracion = medicamentoCLS.Concentracion;
						medicamento.Iidformafarmaceutica = medicamentoCLS.IdFormaFarmaceutica;
						medicamento.Precio = medicamentoCLS.Precio;
						medicamento.Stock = medicamentoCLS.Stock;
						medicamento.Presentacion = medicamentoCLS.Presentacion;
						medicamento.Bhabilitado = 1;

						if (medicamentoCLS.IdMedicamento == 0)
							bDHospitalContext.Medicamento.Add(medicamento);

						bDHospitalContext.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				ViewBag.listFormaFarmaceutica = listarFormaFarmaceutica();
				return View(vista,medicamentoCLS);
			}

			return RedirectToAction("Index");
		}

		public IActionResult Editar(int id)
		{
			ViewBag.listFormaFarmaceutica = listarFormaFarmaceutica();

			MedicamentoCLS medicamentoCLS = new MedicamentoCLS();

			using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
			{
				medicamentoCLS = bDHospitalContext.Medicamento
						.Where(m => m.Iidmedicamento == id).Select(m => new MedicamentoCLS
						{
							IdMedicamento = m.Iidmedicamento,
							Nombre = m.Nombre,
							Concentracion = m.Concentracion,
							IdFormaFarmaceutica = m.Iidformafarmaceutica,
							Precio = m.Precio,
							Stock = m.Stock,
							Presentacion = m.Presentacion
				}).First();
			}

			return View(medicamentoCLS);
		}

		public int Eliminar(int idMedicamento)
		{
			int resultado = 0;
			try
			{
				using (BDHospitalContext bDHospitalContext = new BDHospitalContext())
				{
					Medicamento medicamento = bDHospitalContext.Medicamento.Where(m => m.Iidmedicamento == idMedicamento).First();
					bDHospitalContext.Medicamento.Remove(medicamento);
					bDHospitalContext.SaveChanges();
					resultado = 1;
				}
			}
			catch (Exception ex){}


			return resultado;
		}
	}
}
