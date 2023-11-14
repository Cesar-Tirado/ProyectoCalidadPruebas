using ProyectoTest.Logica;
using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoTest.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string NCorreo, string NContrasena)
        {            
            Usuario oUsuario = new Usuario();
            oUsuario = UsuarioLogica.Instancia.Obtener(NCorreo, NContrasena);

            if (oUsuario != null)
            {
                FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);
                Session["Usuario"] = oUsuario;

                if (oUsuario.EsAdministrador == true)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Tienda");
                }
            }
            else
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }
        }

        // GET: Login
        public ActionResult Registrarse()
        {
            return View(new Usuario() { Nombres= "",Apellidos= "",Correo="",Contrasena="",ConfirmarContrasena="" });
        }

        [HttpPost]
        public ActionResult Registrarse(string NNombres, string NApellidos, string NCorreo, string NContrasena, string NConfirmarContrasena)
        {
            Usuario oUsuario = new Usuario()
            {
                Nombres = NNombres,
                Apellidos = NApellidos,
                Correo = NCorreo,
                Contrasena = NContrasena,
                ConfirmarContrasena = NConfirmarContrasena,
                EsAdministrador = false
            };

            if (NContrasena != NConfirmarContrasena)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View(oUsuario);
            }
            else {
                

                int idusuario_respuesta = UsuarioLogica.Instancia.Registrar(oUsuario);

                if (idusuario_respuesta == 0)
                {
                    ViewBag.Error = "Error al registrar";
                    return View();

                }
                else {
                    return RedirectToAction("Index", "Login");
                }
            }
        }

        [HttpPost]
        public ActionResult RegistrarCliente(string Nombre, string Apellidos, string Email, string Telefono, string Cumpleanos, string Distrito, int Calificacion, bool RecibirPromociones)
        {
            Cliente cliente = new Cliente
            {
                Nombre = Nombre,
                Apellidos = Apellidos,
                Email = Email,
                Telefono = Telefono,
                Cumpleanos = Cumpleanos,
                Distrito = Distrito,
                Calificacion = Calificacion,
                RecibirPromociones = RecibirPromociones
            };

            // Llama a la lógica de negocios para registrar al cliente
            UsuarioLogica clienteLogica = new UsuarioLogica();
            int idClienteRegistrado = clienteLogica.RegistrarCliente(cliente);

            if (idClienteRegistrado > 0)
            {
                CorreoLogica.Instancia.EnviarVale(idClienteRegistrado, Email);
                // El registro fue exitoso, redirige a una página de confirmación o a donde desees.
                return RedirectToAction("Index", "Login");
            }
            else
            {
                // El registro falló, puedes manejar errores aquí.
                ViewBag.Error = "Error al registrar";
                return View(); // Puedes redirigir a la página de registro nuevamente.
            }
        }

    }

}