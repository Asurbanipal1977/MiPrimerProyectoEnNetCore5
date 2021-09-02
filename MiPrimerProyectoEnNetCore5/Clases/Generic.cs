using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MiPrimerProyectoEnNetCore5.Models;

namespace MiPrimerProyectoEnNetCore5.Clases
{
	public class Generic
	{
		public static string CifrarDatos(string datos)
		{
			SHA256Managed sh = new SHA256Managed();
			byte[] arraySinCifrar = Encoding.ASCII.GetBytes(datos);
			byte[] arrayCifrado = sh.ComputeHash(arraySinCifrar);
			return BitConverter.ToString(arrayCifrado).Replace("-", "");
		}

		public static int ObtenerTipoVista(string nomPagina, int idUsuario)
		{
			using (BDHospitalContext bd = new BDHospitalContext())
			{
				var idVista = (from tup in bd.TipoUsuarioPagina
							   join tu in bd.TipoUsuario on tup.Iidtipousuario equals tu.Iidtipousuario
							   join u in bd.Usuario on tup.Iidtipousuario equals u.Iidtipousuario
							   join p in bd.Pagina on tup.Iidpagina equals p.Iidpagina
							   where u.Bhabilitado == 1 && tup.Bhabilitado == 1 && tu.Bhabilitado == 1 &&
							   u.Iidusuario == idUsuario && p.Controlador.ToUpper() == nomPagina.ToUpper()
							   select tup.Iidvista).First().Value;
				return idVista;
			}
		}


		public static List<PaginaCLS> ListarPaginasControlador(string controlador)
		{
			if (listaBotonesPagina != null)
				return listaBotonesPagina.Where(bp => bp.Controlador.ToUpper().Trim() == controlador.ToUpper().Trim()).ToList();
			else return null;
		}


		public static List<PaginaCLS> ListaPaginas { get; set; }

		public static List<PaginaCLS> listaBotonesPagina { get; set; }

		
	}
}
