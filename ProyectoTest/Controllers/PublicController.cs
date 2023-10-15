using ProyectoTest.Logica;
using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoTest.Controllers
{
    public class PublicController : Controller
    {
        private static Usuario oUsuario;
        //VISTA
        public ActionResult Index()
        {
            oUsuario = new Usuario();
            oUsuario.Correo = "cesar@admin.com";
            oUsuario.IdUsuario = 2;
            oUsuario.Nombres = "cesar";
            Session["Usuario"] = oUsuario;
            return View();
        }

    }

}