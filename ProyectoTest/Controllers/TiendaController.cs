using Newtonsoft.Json;
using ProyectoTest.Logica;
using ProyectoTest.Models;
using ProyectoTest.Models.Niubiz.Consultas;
using ProyectoTest.Models.Niubiz.Respuestas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace ProyectoTest.Controllers
{
    public class TiendaController : Controller
    {
        private static Usuario oUsuario;
        public tarjetaResponse datosTar = new tarjetaResponse();
        //VISTA
        public ActionResult Index()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");
            else
                oUsuario = (Usuario)Session["Usuario"];
            return View();
        }

        //VISTA
        public ActionResult Producto(int idproducto = 0)
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");
            else
                oUsuario = (Usuario)Session["Usuario"];

            Producto oProducto = new Producto();
            List<Producto> oLista = new List<Producto>();

            oLista = ProductoLogica.Instancia.Listar();
            oProducto = (from o in oLista
                         where o.IdProducto == idproducto
                         select new Producto()
                         {
                             IdProducto = o.IdProducto,
                             Nombre = o.Nombre,
                             Descripcion = o.Descripcion,
                             OpcionesConCosto = o.OpcionesConCosto,
                             OpcionesSinCosto = o.OpcionesSinCosto,
                             OpcionExcluyente = o.OpcionExcluyente,
                             MaxOpcionesSinCosto = o.MaxOpcionesSinCosto,
                             oMarca = o.oMarca,
                             oCategoria = o.oCategoria,
                             Precio = o.Precio,
                             RutaImagen = o.RutaImagen,
                             base64 = utilidades.convertirBase64(Server.MapPath(o.RutaImagen)),
                             extension = Path.GetExtension(o.RutaImagen).Replace(".", ""),
                             Activo = o.Activo,
                         }).FirstOrDefault();

            return View(oProducto);
        }

        //VISTA
        public ActionResult Carrito()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");
            else
                oUsuario = (Usuario)Session["Usuario"];
            return View();
        }

        //VISTA
        public ActionResult Compras()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");
            else
                oUsuario = (Usuario)Session["Usuario"];
            return View();
        }


        [HttpPost]
        public JsonResult ListarProducto(int idcategoria = 0, string nombre = "")
        {
            List<Producto> oLista = new List<Producto>();
            oLista = ProductoLogica.Instancia.Listar();
           

            if (idcategoria != 0)
            {
                oLista = oLista.Where(x => x.oCategoria.IdCategoria == idcategoria).ToList();
            }
            else if (nombre != "")
            {
                oLista = oLista.Where(x => x.Nombre.ToLower().Contains(nombre.ToLower())).ToList();
            }
            oLista = oLista.Where(x => x.Activo == true).ToList();
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }


        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = CategoriaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarTiendas()
        {
            List<Tiendas> oLista = new List<Tiendas>();
            oLista = TiendasLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ListarTiendasActivo()
        {
            List<Tiendas> oLista = new List<Tiendas>();
            oLista = TiendasLogica.Instancia.ListarTiendasActivo();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertarCarrito(int IdUsuario, int IdProducto, int Cantidad, string Adicionales, decimal PrecioExtra, string Observaciones)
        {
            int _respuesta = 0;
            Carrito oCarrito = new Carrito()
            {
                oUsuario = new Usuario() { IdUsuario = IdUsuario },
                oProducto = new Producto() { IdProducto = IdProducto },
                Cantidad = Cantidad,
                Adicionales = Adicionales,
                PrecioExtra = PrecioExtra,
                Observaciones = Observaciones
            };
            _respuesta = CarritoLogica.Instancia.Registrar(oCarrito);
            return Json(new { respuesta = _respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadCarrito()
        {
            int _respuesta = 0;
            if (oUsuario != null)
                _respuesta = CarritoLogica.Instancia.Cantidad(oUsuario.IdUsuario);
            return Json(new { respuesta = _respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ObtenerCarrito()
        {
            List<Carrito> oLista = CarritoLogica.Instancia.Obtener(oUsuario.IdUsuario);

            if (oLista.Count != 0)
            {
                oLista = (from d in oLista
                          select new Carrito()
                          {
                              IdCarrito = d.IdCarrito,
                              oProducto = new Producto()
                              {
                                  IdProducto = d.oProducto.IdProducto,
                                  Nombre = d.oProducto.Nombre,
                                  oMarca = new Marca() { Descripcion = d.oProducto.oMarca.Descripcion },
                                  Precio = d.oProducto.Precio,
                                  RutaImagen = d.oProducto.RutaImagen,
                                  base64 = utilidades.convertirBase64(Server.MapPath(d.oProducto.RutaImagen)),
                                  extension = Path.GetExtension(d.oProducto.RutaImagen).Replace(".", ""),
                              },
                              Cantidad = d.Cantidad,
                              Adicionales = d.Adicionales,
                              PrecioExtra = d.PrecioExtra,
                              Observaciones = d.Observaciones,
                              PrecioEnvio = d.PrecioEnvio
                          }).ToList();
            }
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var json = Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }


        [HttpPost]
        public JsonResult EliminarCarrito(string IdCarrito, string IdProducto)
        {
            bool respuesta = false;
            respuesta = CarritoLogica.Instancia.Eliminar(IdCarrito, IdProducto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
              

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session["Usuario"] = null;
            return RedirectToAction("Index", "Public");
        }


        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();
            oLista = UbigeoLogica.Instancia.ObtenerDepartamento();
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerProvincia(string _IdDepartamento)
        {
            List<Provincia> oLista = new List<Provincia>();
            oLista = UbigeoLogica.Instancia.ObtenerProvincia(_IdDepartamento);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDistrito(string _IdProvincia, string _IdDepartamento)
        {
            List<Distrito> oLista = new List<Distrito>();
            oLista = UbigeoLogica.Instancia.ObtenerDistrito(_IdProvincia, _IdDepartamento);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RegistrarCompra(Compra oCompra)
        {
            bool respuesta = false;
            oCompra.IdUsuario = oUsuario.IdUsuario;            
            respuesta = CompraLogica.Instancia.Registrar(oCompra);

            if (respuesta)
            {
                CorreoLogica.Instancia.EnviarCorreo(oCompra);

                return Json(new { resultado = true, mensaje = "Compra registrada exitosamente, se le enviará un correo con su pedido." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { resultado = false, mensaje = "No se pudo registrar la compra." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult CambiarEstadoCompra(int idCompra, string nuevoEstado)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.CN))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("sp_actualizarEstadoCompra", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCompra", idCompra);
                    cmd.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                    cmd.ExecuteNonQuery();
                    return Json(new { success = true, message = "Estado de compra actualizado correctamente."});
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al actualizar el estado de compra.", ex});
            }
        }

        [HttpGet]
        public JsonResult ObtenerCompraUsr()
        {
            List<Compra> oLista = new List<Compra>();

            oLista = CarritoLogica.Instancia.ObtenerCompraUsuario(oUsuario.IdUsuario);

            oLista = (from c in oLista
                      select new Compra()
                      {
                          IdCompra = c.IdCompra,
                          Estado = c.Estado,
                          Referencia = c.Referencia,
                          FormaPago = c.FormaPago,
                          Nombre = c.Nombre,
                          DocumentoFacturacion = c.DocumentoFacturacion,
                          Telefono = c.Telefono,
                          Direccion = c.Direccion,
                          Correo = c.Correo,
                          Total = c.Total,
                          FechaTexto = c.FechaTexto,
                          HoraRecojo = c.HoraRecojo,
                          Tipo = c.Tipo,
                          oDetalleCompra = (from dc in c.oDetalleCompra
                                            select new DetalleCompra()
                                            {
                                                oProducto = new Producto()
                                                {
                                                    oMarca = new Marca() { Descripcion = dc.oProducto.oMarca.Descripcion },
                                                    Nombre = dc.oProducto.Nombre,
                                                    RutaImagen = dc.oProducto.RutaImagen,
                                                    base64 = utilidades.convertirBase64(Server.MapPath(dc.oProducto.RutaImagen)),
                                                    extension = Path.GetExtension(dc.oProducto.RutaImagen).Replace(".", ""),
                                                },
                                                Adicionales = dc.Adicionales,
                                                Total = dc.Total,
                                                PrecioExtra = dc.PrecioExtra,
                                                Cantidad = dc.Cantidad,
                                                ObservacionesDC = dc.ObservacionesDC
                                            }).ToList()
                      }).ToList();

            oLista = oLista.OrderByDescending(c => c.FechaTexto).ToList();

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            var json = Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }

        [HttpGet]
        public JsonResult ObtenerCompra()
        {
            List<Compra> oLista = new List<Compra>();

            oLista = CarritoLogica.Instancia.ObtenerCompra();

            oLista = (from c in oLista
                      select new Compra()
                      {
                          IdCompra = c.IdCompra,
                          Estado = c.Estado,
                          Referencia = c.Referencia,
                          FormaPago = c.FormaPago,
                          Nombre = c.Nombre,
                          DocumentoFacturacion = c.DocumentoFacturacion,
                          Telefono = c.Telefono,
                          Direccion = c.Direccion,
                          Correo = c.Correo,
                          Total = c.Total,
                          FechaTexto = c.FechaTexto,
                          HoraRecojo = c.HoraRecojo,
                          Tipo = c.Tipo,
                          oDetalleCompra = (from dc in c.oDetalleCompra
                                            select new DetalleCompra()
                                            {
                                                oProducto = new Producto()
                                                {
                                                    oMarca = new Marca() { Descripcion = dc.oProducto.oMarca.Descripcion },
                                                    Nombre = dc.oProducto.Nombre,
                                                    RutaImagen = dc.oProducto.RutaImagen,
                                                    base64 = utilidades.convertirBase64(Server.MapPath(dc.oProducto.RutaImagen)),
                                                    extension = Path.GetExtension(dc.oProducto.RutaImagen).Replace(".", ""),
                                                },
                                                Adicionales = dc.Adicionales,
                                                Total = dc.Total,
                                                PrecioExtra = dc.PrecioExtra,
                                                Cantidad = dc.Cantidad,
                                                ObservacionesDC = dc.ObservacionesDC
                                            }).ToList()
                      }).ToList();

            oLista = oLista.OrderByDescending(c => c.FechaTexto).ToList();

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            var json = Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }


        [HttpGet]
        [AllowAnonymous]
        public JsonResult ObtenerProximoIdCompra()
        {
            int proximoIdCompra = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT MAX(idCompra) FROM COMPRA", oConexion);
                    oConexion.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        proximoIdCompra = Convert.ToInt32(result) + 1;
                    }
                }
                catch (Exception ex)
                {
                    proximoIdCompra = 0;
                }
            }
            return Json(proximoIdCompra, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("apis/webhook/{id}")]
        public void apiswebhook(string id, tarjetaResponse dato)
        {
            try
            {
                var dataid = id;
                datosTar = dato;
            }
            catch (Exception ex) { throw ex; }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Login()
        {
            string url = "https://apisandbox.vnforappstest.com/api.security/v1/security";
            string credenciales = Convert.ToBase64String(Encoding.UTF8.GetBytes("integraciones.visanet@necomplus.com:d5e7nk$M"));
            return await PostDataAsync(url, "Basic " + credenciales, null);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Autorize([System.Web.Http.FromBody] NiubizData data, string key)
        {
      
            string merchantId = "522591303";
            string url = $"https://apisandbox.vnforappstest.com/api.ecommerce/v2/ecommerce/token/session/{merchantId}";
            return await PostDataAsync(url, key, JsonConvert.SerializeObject(data));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Pay([System.Web.Http.FromBody] PAY data, string key)
        {
   
            string merchantId = "522591303";
            string url = $"https://apisandbox.vnforappstest.com/api.authorization/v3/authorization/ecommerce/{merchantId}";
            return await PostDataAsync(url, key, JsonConvert.SerializeObject(data));
        }

        [NonAction]
        public static async Task<string> PostDataAsync(string url, string token, string postData)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("Authorization", token);
                if (postData != null)
                {
                    request.Content = new StringContent(postData, Encoding.UTF8, "application/json");
                }
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception($"Error en la solicitud. Código de estado: {response.StatusCode}");
                }
            }
        }
    }
}

public class Antifraud
{
    public string clientIp { get; set; }
    public MerchantDefineData merchantDefineData { get; set; }
}

public class MerchantDefineData
{
    public string MDD4 { get; set; }
    public int MDD21 { get; set; }
    public string MDD32 { get; set; }
    public string MDD75 { get; set; }
    public int MDD77 { get; set; }
}
public class dataEnviadaDesdeFront
{
    public string Token { get; set; }
    public NiubizData Data { get; set; }
}
public class NiubizData
{
    public string channel { get; set; }
    public double amount { get; set; }
    public double recurrenceMaxAmount { get; set; }
    public Antifraud antifraud { get; set; }
}

//pay DTO
public class CardHolder
{    public string documentType { get; set; }
    public string documentNumber { get; set; }
}

public class Order
{
    public string tokenId { get; set; }
    public int purchaseNumber { get; set; }
    public double amount { get; set; }
    public string currency { get; set; }
    public string productId { get; set; }
    public int installment { get; set; } = 0;
}

public class Recurrence
{
    public string type { get; set; }
    public string frequency { get; set; }
    public string beneficiaryId { get; set; }
    public string beneficiaryFirstName { get; set; }
    public string beneficiaryLastName { get; set; }
    public double maxAmount { get; set; }
    public double amount { get; set; }
}

public class PAY
{
    public string channel { get; set; }
    public string captureType { get; set; }
    public bool countable { get; set; }
    public Order order { get; set; }
    public CardHolder cardHolder { get; set; }
    public Recurrence recurrence { get; set; }
}

