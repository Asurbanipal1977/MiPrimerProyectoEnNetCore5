using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiPrimerProyectoEnNetCore5.Clases;
using MiPrimerProyectoEnNetCore5.Models;

namespace MiPrimerProyectoEnNetCore5.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult MiPagina()
		{
			return View();
		}

		public int Entero()
		{
			return 10;
		}

		public string Saludo(Instructor instructor)
		{
			return $"Hola Mundo {instructor.Nombre} {instructor.ApellidoPaterno} {instructor.ApellidoMaterno}";
		}

		public List<Instructor> mostrarInstructor()
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

			return lInstructor;
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
