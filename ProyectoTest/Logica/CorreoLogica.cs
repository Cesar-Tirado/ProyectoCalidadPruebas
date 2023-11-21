using ProyectoTest.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web.UI.WebControls;
using System.Text;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Web.Caching;
using System.Web.Razor.Parser.SyntaxTree;
using System.Net.Mime;

namespace ProyectoTest.Logica
{
    public class CorreoLogica
    {
        private static CorreoLogica _instancia = null;

        public CorreoLogica()
        {

        }

        public static CorreoLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CorreoLogica();
                }

                return _instancia;
            }
        }



        public bool EnviarCorreo(Compra oCompra)
        {
            bool sendMail = false; 

            try
            {
                AppSettingsReader DatosConexion = new AppSettingsReader();

                string mail = DatosConexion.GetValue("mail", typeof(string)).ToString();
                string pass = DatosConexion.GetValue("pass", typeof(string)).ToString();
                //string mailCopy = DatosConexion.GetValue("mailCopy", typeof(string)).ToString();
                string idCompraFormateado = oCompra.IdCompra.ToString("D8");

                decimal precioTotal = oCompra.Total;
                string DireccionTienda = "";
                string DireccionCliente = "";
                if (oCompra.Tipo == "Recojo")
                {
                    DireccionTienda = oCompra.Direccion;
                    DireccionCliente = "";
                    oCompra.Tipo = "Recojo en Tienda";

                } else if ( oCompra.Tipo == "Delivery")
                {
                    DireccionTienda = "Calle Ernesto Plascencia 300, San Isidro";
                    DireccionCliente = oCompra.Direccion;
                }  

                string imagen = "~/Imagenes/LongHorn/cachitorojo.png";
                StringBuilder query = new StringBuilder();
                string tablaHTML = "<table><tr><th class='columna-derecha' style='padding-right: 100px'>Detalle de tu pedido:</th><th class='columna-derecha'></th></tr>";

                foreach (DetalleCompra dc in oCompra.oDetalleCompra)
                {
                    string precioExtraFormat = dc.PrecioExtra.ToString("0.00");

                    tablaHTML += "<tr>";
                    tablaHTML += "<td class='columna-izquierda'><b>" + dc.Cantidad + " x " + " " + dc.Nombre + "</b></td>";
                    tablaHTML += "<td class='columna-derecha' style='width:100px;'><b>" + "S/. " + precioExtraFormat + "</b></td>";
                    tablaHTML += "</tr>";
                    tablaHTML += "<tr>";
                    tablaHTML += "<td class='columna-izquierda' style='padding-left:30px;'>" + dc.Adicionales + "</td>";
                    tablaHTML += "<td class='columna-izquierda' style='padding-left:30px;'>" + dc.ObservacionesDC + "</td>";
                    tablaHTML += "<td class='columna-derecha'></td>";
                    tablaHTML += "</tr>";                   
                }
                tablaHTML += "</ table >";

                MailMessage Correos = new MailMessage();
                SmtpClient envio = new SmtpClient();

                Correos.To.Clear();
                Correos.Attachments.Clear();
                //Correos.Bcc.Add(mailCopy);
                Correos.Subject = "Pedido";
                Correos.Body = @"
                    <html>
                    <head>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;            
                            }        
                            .container {
                                max-width: 600px;
                                text-align: center;
                                margin: 0 auto;
                                padding: 20px;
                                background-color: #ffffff;
                                border-radius: 5px;
                                box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
                            }
                            h1, h5 {
                                color: #333333;
                                padding: 0px;
                                margin: 5px;
                            }
                            p {
                                color: #666666;
                            }

                            .container2 {
                                width: 600px;
                                margin: 0 auto;
                            }

                            .columna-izquierda {
                                text-align: left;
                            }
        
                            .columna-derecha {
                                text-align: right!important;
                            }

                            table {
                                font-size: 12px;
                                margin: 0px;
                                width: 600px;
                                text-align: left;
                                border-collapse: collapse;
                            }

                            th {
                                font-size: 14px;
                                font-weight:bold;
                                padding: 8px;
                                background: #d3d3d3;
                            }

                            td {
                                padding: 8px;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <img src = " + imagen + @" alt = '' width='200px'>
                            <p>¡Hola " + oCompra.Nombre + @", tu pedido ha sido confirmado!</p>
                            <h5>Su orden tiene el ID: N° " + idCompraFormateado + @" </h5>
                            <h5>Su pedido se programó para : " + oCompra.HoraRecojo + @" </h5><br>
                            <span style='font-size:14px; font-weight:bold; color:black'> Su pedido: " + oCompra.Tipo + @"  </span><hr>

                            <div class='container2'>
                                <table>
                                    <tr>
                                        <th class='columna-izquierda'>Datos del Local</td>
                                        <th class='columna-derecha'>Datos del Cliente</td>
                                    </tr>
                                    <tr>
                                      
                                        <td class='columna-derecha'> " + oCompra.Nombre + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='columna-izquierda'>" + DireccionTienda + @"</td>
                                        <td class='columna-derecha'>" + DireccionCliente + @"</td>
                                    </tr>   
                                    <tr>
                        
                                        <td class='columna-derecha'> " + oCompra.DocumentoFacturacion + @"</td>
                                    </tr>  
                                    <tr>
                        
                                        <td class='columna-derecha'> " + oCompra.Telefono + @"</td>
                                    </tr> 
                                    <tr>
    
                                        <td class='columna-derecha'> " + oCompra.Correo + @" </td>
                                    </tr>                            
                                </table>
                                <hr> 
                            </div>

                            <div class='container2'>
                               " + tablaHTML + @"                             
                            </div>

                            <div class='container2'>
                                <table>
                                    <tr>
                                        <th class='columna-izquierda'>Total Parcial: </td>
                                        <th class='columna-derecha'> S/." + oCompra.Total + @"</td>
                                    </tr>
                                    <tr>
                                        <th class='columna-izquierda'>Costo de envío: </td>
                                        <th class='columna-derecha'> S/.0.00  </td>
                                    </tr>
                                    <tr>
                                        <th class='columna-izquierda' style='font-size:24px;'>Total: </td>
                                        <th class='columna-derecha' style='font-size:24px;'> S/. " + oCompra.Total + @"</td>
                                    </tr>
                                    <tr>
                                        <td class='columna-izquierda' style='padding-top: 20px;'>Método de Pago: " + oCompra.FormaPago + @" </td>
                                        <td class='columna-derecha' style='padding-top: 20px;'>Método de entrega: " + oCompra.Tipo + @" </td>
                                    </tr>
                                </table>            
                            </div>        
                        </div>
                    </body>
                    </html>";

                Correos.IsBodyHtml = true;
                Correos.To.Add(oCompra.Correo); // Destino

                Correos.From = new MailAddress(mail);
                envio.Credentials = new NetworkCredential(mail, pass);

                envio.Host = "smtp.gmail.com";
                envio.Port = 587;
                envio.EnableSsl = true;
                envio.Send(Correos);
                sendMail = true;
                Console.WriteLine("Correo Enviado");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " +ex.Message + ex.ToString());
            }
            return sendMail;
        }


        public bool EnviarVale(int IdCliente, string Email)
        {
            bool sendMail = false;

            try
            {
                AppSettingsReader DatosConexion = new AppSettingsReader();
                string mail = DatosConexion.GetValue("mail", typeof(string)).ToString();
                string pass = DatosConexion.GetValue("pass", typeof(string)).ToString();

                MailMessage Correos = new MailMessage();
                SmtpClient envio = new SmtpClient();

                Correos.To.Clear();
                Correos.Attachments.Clear();
                Correos.Subject = "Correo con Imagen y Adjunto";

                // Ruta de la imagen embebida
                string imagePath = @"C:\Users\cesar\OneDrive\Documentos\Proyectos\PRUEBAS_BACKUPS\LongHorn_Carrito\Ecommerce\ProyectoTest\Imagenes\LongHorn/Billete_LHG.png"; // Reemplaza con la ruta a tu imagen

                // Agrega la imagen embebida
                AlternateView av = GetEmbeddedImage(imagePath);

                string htmlBody = $@"
            <html>
                <body>
                    <h1>Cliente con Id: {IdCliente}</h1>
                    <img src='cid:{av.LinkedResources[0].ContentId}' alt='' width='200px'>
                </body>
            </html>
        ";

                av.LinkedResources[0].ContentLink = new Uri(imagePath);

                Correos.AlternateViews.Add(av);
                Correos.IsBodyHtml = true;
                Correos.To.Add(Email);

                // Adjunta la imagen como archivo
                Attachment imagenAdjunta = new Attachment(imagePath);
                Correos.Attachments.Add(imagenAdjunta);

                Correos.From = new MailAddress(mail);
                envio.Credentials = new NetworkCredential(mail, pass);

                envio.Host = "smtp.gmail.com";
                envio.Port = 587;
                envio.EnableSsl = true;
                envio.Send(Correos);
                sendMail = true;
                Console.WriteLine("Correo con imagen y archivo adjunto enviado");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message + ex.ToString());
            }
            return sendMail;
        }

        private AlternateView GetEmbeddedImage(string filePath)
        {
            LinkedResource res = new LinkedResource(filePath);
            res.ContentId = Guid.NewGuid().ToString();
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString("", null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
    }
}