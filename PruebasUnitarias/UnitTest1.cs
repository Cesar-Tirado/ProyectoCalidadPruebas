using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ProyectoTest;
using System.Web.Http.Results;
using ProyectoTest.Models;
using System.Web.Mvc;
using ProyectoTest.Controllers;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Collections.Generic;

namespace PruebasUnitarias
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Stream _stream;



        [TestMethod]
        public void Registrarse_ContraseñasCoinciden_RedireccionaAIndex()
        {
            // Arrange
            var controller = new ProyectoTest.Controllers.LoginController(); // Reemplaza 'TuControlador' con el nombre real de tu controlador.

            //cambiar los datos para cuando presente
            var modelo = new Usuario()
            {
                Nombres = "Examen2",
                Apellidos = "Calidad2",
                Correo = "examen2@calidad.com",
                Contrasena = "password2",
                ConfirmarContrasena = "password2"
            };

            // Act
            var resultado = controller.Registrarse(modelo.Nombres, modelo.Apellidos, modelo.Correo, modelo.Contrasena, modelo.ConfirmarContrasena);

            // Assert
            Assert.IsInstanceOfType(resultado, typeof(System.Web.Mvc.RedirectToRouteResult));
        }

        //[TestMethod]
        //public void CambiarEstadoCompra_DebeRetornarJsonSuccessTrue()
        //{
        //    // Arrange
        //    var controller = new ProyectoTest.Controllers.TiendaController();
        //    int idCompra = 1;
        //    string nuevoEstado = "Aprobado";

        //    // Act
        //    var resultado = controller.CambiarEstadoCompra(idCompra, nuevoEstado) as JsonResult;

        //    // Assert
        //    Assert.IsNotNull(resultado);
        //    dynamic data = resultado.Data;
        //    Assert.IsNotNull(data);
        //    Assert.AreEqual(true, data.success);
        //    Assert.AreEqual("Estado de compra actualizado correctamente.", data.message);
        //}

        //[TestMethod]
        //public void CambiarEstadoCompra_DebeRetornarJsonSuccessFalse()
        //{
        //    // Arrange
        //    var controller = new ProyectoTest.Controllers.TiendaController();
        //    int idCompra = -1; 
        //    string nuevoEstado = "Aprobado";

        //    // Act
        //    var resultado = controller.CambiarEstadoCompra(idCompra, nuevoEstado) as JsonResult;

        //    // Assert
        //    Assert.IsNotNull(resultado);
        //    dynamic data = resultado.Data;
        //    Assert.IsNotNull(data);
        //    Assert.AreEqual(false, data.success);
        //    Assert.AreEqual("Error al actualizar el estado de compra.", data.message);
        //}

        //[TestMethod]
        //public void EliminarMarca_DebeRetornarJsonExitoso()
        //{
        //    // Arrange
        //    var controller = new ProyectoTest.Controllers.HomeController(); // Reemplaza 'TuControlador' con el nombre real de tu controlador.
        //    int idMarca = 26; // Proporciona un valor válido de idMarca para una marca que deseas eliminar.


        //    // Act
        //    var resultado = controller.EliminarMarca(idMarca) as JsonResult;

        //    // Assert
        //    Assert.IsNotNull(resultado);
        //    dynamic data = resultado.Data;
        //    Assert.AreEqual(true, data.resultado);
        //}











    }
}
