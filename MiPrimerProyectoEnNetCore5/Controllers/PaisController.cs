using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiPrimerProyectoEnNetCore5.Clases;

namespace MiPrimerProyectoEnNetCore5.Controllers
{
	public class PaisController : BaseController
	{
		public IActionResult Inicio()
		{
			return View();
		}

		public IActionResult Lista()
		{
			return View();
		}

		public IActionResult PruebaLista()
		{
			List<Instructor> lInstructor = new List<Instructor>();

			lInstructor.Add(
			new Instructor()
			{
				Nombre = "Iván",
				ApellidoPaterno = "Martínez",
				ApellidoMaterno = "Santos"
			});


			lInstructor.Add(
			new Instructor()
			{
				Nombre = "Miguel Ángel",
				ApellidoPaterno = "Martínez",
				ApellidoMaterno = "Eusebio"
			});

			return View(lInstructor);
		}
	}
}
