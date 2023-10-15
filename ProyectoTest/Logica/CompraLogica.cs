using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoTest.Logica
{
    public class CompraLogica
    {
        private static CompraLogica _instancia = null;

        public CompraLogica()
        {

        }

        public static CompraLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CompraLogica();
                }

                return _instancia;
            }
        }

        public bool Registrar(Compra oCompra)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    foreach (DetalleCompra dc in oCompra.oDetalleCompra)
                    {
                        query.AppendLine("INSERT INTO detalle_compra (IdCompra, IdProducto, Cantidad, Total, Nombre, Adicionales) VALUES (¡idcompra!," + dc.IdProducto + "," + dc.Cantidad + "," + dc.Total + ",'" + dc.Nombre + "','" + dc.Adicionales + "')");
                    }

                    SqlCommand cmd = new SqlCommand("sp_registrarCompra", oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario", oCompra.IdUsuario);
                    cmd.Parameters.AddWithValue("TotalProducto", oCompra.TotalProducto);
                    cmd.Parameters.AddWithValue("Total", oCompra.Total);
                    cmd.Parameters.AddWithValue("Direccion", oCompra.Direccion);
                    cmd.Parameters.AddWithValue("Nombre", oCompra.Nombre);
                    cmd.Parameters.AddWithValue("Telefono", oCompra.Telefono);
                    cmd.Parameters.AddWithValue("Correo", oCompra.Correo);
                    cmd.Parameters.AddWithValue("Referencia", oCompra.Referencia);
                    cmd.Parameters.AddWithValue("DocumentoFacturacion", oCompra.DocumentoFacturacion);
                    cmd.Parameters.AddWithValue("FormaPago", oCompra.FormaPago);
                    cmd.Parameters.AddWithValue("QueryDetalleCompra", query.ToString());
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        //public bool Registrar(Compra oCompra)
        //{
        //    bool respuesta = false;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            StringBuilder query = new StringBuilder();
        //            foreach (DetalleCompra dc in oCompra.oDetalleCompra)
        //            {
        //                query.AppendLine("INSERT INTO detalle_compra (IdCompra, IdProducto, Cantidad, Total) VALUES (¡idcompra!," + dc.IdProducto + "," + dc.Cantidad + "," + dc.Total + ")");
        //            }

        //            SqlCommand cmd = new SqlCommand("sp_registrarCompra", oConexion);
        //            cmd.Parameters.AddWithValue("IdUsuario", oCompra.IdUsuario);
        //            cmd.Parameters.AddWithValue("TotalProducto", oCompra.TotalProducto);
        //            cmd.Parameters.AddWithValue("Total", oCompra.Total);
        //            cmd.Parameters.AddWithValue("Contacto", oCompra.Contacto);
        //            cmd.Parameters.AddWithValue("Telefono", oCompra.Telefono);
        //            cmd.Parameters.AddWithValue("Direccion", oCompra.Direccion);
        //            cmd.Parameters.AddWithValue("IdDistrito", oCompra.IdDistrito);
        //            //cmd.Parameters.AddWithValue("Estado", "PENDIENTE");// Estado por defecto es PENDIENTE
        //            cmd.Parameters.AddWithValue("QueryDetalleCompra", query.ToString());
        //            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            oConexion.Open();
        //            cmd.ExecuteNonQuery();
        //            respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
        //        }
        //        catch (Exception ex)
        //        {
        //            respuesta = false;
        //        }
        //    }
        //    return respuesta;
        //}

        public bool ActualizarEstadoCompra(int idCompra, string nuevoEstado)
        {
            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
                {
                    SqlCommand cmd = new SqlCommand("sp_actualizarEstadoCompra", oConexion);
                    cmd.Parameters.AddWithValue("IdCompra", idCompra);
                    cmd.Parameters.AddWithValue("Estado", nuevoEstado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    bool resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción o lanzarla según el manejo de errores de tu aplicación
                throw ex;
            }
        }



    }
}