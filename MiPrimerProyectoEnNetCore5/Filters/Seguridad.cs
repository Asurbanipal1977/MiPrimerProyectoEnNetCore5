using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MiPrimerProyectoEnNetCore5.Filters
{
	public class Seguridad : IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
			return;
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var usuario = context.HttpContext.Session.GetString("usuario");
			if (usuario == null)
			{
				context.Result = new RedirectResult("Login");
			}
		}
	}
}
