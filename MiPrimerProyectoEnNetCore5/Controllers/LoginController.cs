using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiPrimerProyectoEnNetCore5.Clases;
using MiPrimerProyectoEnNetCore5.Models;

namespace MiPrimerProyectoEnNetCore5.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Cerrar()
		{
			HttpContext.Session.Remove("usuario");
			return RedirectToAction("Index");
		}

		public string login(string user, string password)
		{
			string respuesta = "KO";
			Usuario usuario = null;
			using (BDHospitalContext bdContext = new BDHospitalContext())
			{
				string claveCifrada = Generic.CifrarDatos(password);
				usuario = bdContext.Usuario.Where(u => u.Nombreusuario == user && u.Contraseña == claveCifrada).FirstOrDefault();
				if (usuario != null)
				{
					respuesta = "OK";
					HttpContext.Session.SetString("usuario", usuario.Iidusuario.ToString());
					int idTipoUsuario = usuario.Iidtipousuario;
					List<PaginaCLS> lista = (from u in bdContext.TipoUsuarioPagina
											join p in bdContext.Pagina on u.Iidpagina equals p.Iidpagina
											where u.Bhabilitado == 1 && u.Iidtipousuario == idTipoUsuario
											select(new PaginaCLS
											{
												IdPagina = (int)u.Iidpagina,
												Mensaje = p.Mensaje,
												Accion = p.Accion,
												Controlador = p.Controlador
											}
											))
										.ToList();

					List<PaginaCLS> listaBotones = (from pgtb in bdContext.TipoUsuarioPaginaBoton
											 join t in bdContext.TipoUsuarioPagina on pgtb.Iidtipousuariopagina equals t.Iidtipousuariopagina
											 join p in bdContext.Pagina on t.Iidpagina equals p.Iidpagina
											 where pgtb.Bhabilitado == 1 && t.Bhabilitado==1 && t.Iidtipousuario == idTipoUsuario
											 select (new PaginaCLS
											 {
												 IdPagina = (int) t.Iidpagina,
												 IdBoton = (int) pgtb.Iidboton,
												 Controlador = p.Controlador
											 }
											 ))
										.ToList();

					Generic.ListaPaginas = lista;
					Generic.listaBotonesPagina = listaBotones;
				}
			}
			return respuesta;
		}
	}
}
