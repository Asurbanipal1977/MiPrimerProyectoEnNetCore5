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
	public class PaginaPrincipalController : BaseController
	{
		public IActionResult Index()
		{
			int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));
			PersonaRegistrarCLS personaCLS = new PersonaRegistrarCLS();

			using (BDHospitalContext bDHospital = new BDHospitalContext())
			{
				int idPersona = (int)bDHospital.Usuario.Where(u => u.Iidusuario == idUsuario).First().Iidpersona;
				Persona persona = bDHospital.Persona.Where(p => p.Iidpersona == idPersona).First();
				personaCLS.Nombre = persona.Nombre;
				personaCLS.Appaterno = persona.Apmaterno;
				personaCLS.Apmaterno = persona.Apmaterno;
				personaCLS.Telefonofijo = persona.Telefonofijo;
				personaCLS.Telefonocelular = persona.Telefonocelular;

				personaCLS.Foto = (persona.Foto==null)? "favicon.ico": persona.Foto;
				personaCLS.Fechanacimiento = persona.Fechanacimiento;

				var consulta = (from u in bDHospital.Usuario
								join t in bDHospital.TipoUsuario on u.Iidtipousuario equals t.Iidtipousuario
								where u.Bhabilitado == 1 && t.Bhabilitado == 1 && u.Iidusuario == idUsuario
								select new PersonaRegistrarCLS {
									nombreUsuario = u.Nombreusuario,
									nombreTipoUsuario = t.Nombre
								}).First();


				personaCLS.nombreUsuario = consulta.nombreUsuario;
				personaCLS.nombreTipoUsuario = consulta.nombreTipoUsuario;



			}

			return View(personaCLS);
		}
	}
}
