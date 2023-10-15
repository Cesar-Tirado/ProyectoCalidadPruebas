using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ProyectoTest.Logica
{
    public class ProductoLogica
    {
        private static ProductoLogica _instancia = null;

        public ProductoLogica()
        {

        }

        public static ProductoLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new ProductoLogica();
                }

                return _instancia;
            }
        }

        public List<Producto> Listar()
        {

            List<Producto> rptListaProducto = new List<Producto>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_obtenerProducto", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaProducto.Add(new Producto()
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"].ToString()),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            OpcionesConCosto = dr["OpcionesConCosto"].ToString(),
                            OpcionesSinCosto = dr["OpcionesSinCosto"].ToString(),
                            OpcionExcluyente = dr["OpcionExcluyente"].ToString(),
                            MaxOpcionesSinCosto = Convert.ToInt32(dr["MaxOpcionesSinCosto"].ToString()), // Agregar el nuevo campo
                            oMarca = new Marca() { IdMarca = Convert.ToInt32(dr["IdMarca"].ToString()), Descripcion = dr["DescripcionMarca"].ToString() },
                            oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(dr["IdCategoria"].ToString()), Descripcion = dr["DescripcionCategoria"].ToString() },
                            Precio = Convert.ToDecimal(dr["Precio"].ToString(), new CultureInfo("es-PE")),
                            RutaImagen = dr["RutaImagen"].ToString(),

                            Activo = Convert.ToBoolean(dr["Activo"].ToString()),
                        });
                    }
                    dr.Close();

                    return rptListaProducto;

                }
                catch (Exception)
                {
                    rptListaProducto = null;
                    return rptListaProducto;
                }
            }
        }



        public int Registrar(Producto oProducto)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrarProducto", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", oProducto.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", oProducto.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", oProducto.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", oProducto.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", oProducto.Precio);
                    cmd.Parameters.AddWithValue("RutaImagen", oProducto.RutaImagen);
                    cmd.Parameters.AddWithValue("OpcionesConCosto", string.IsNullOrEmpty(oProducto.OpcionesConCosto) ? (object)DBNull.Value : oProducto.OpcionesConCosto);
                    cmd.Parameters.AddWithValue("OpcionesSinCosto", string.IsNullOrEmpty(oProducto.OpcionesSinCosto) ? (object)DBNull.Value : oProducto.OpcionesSinCosto);
                    cmd.Parameters.AddWithValue("OpcionExcluyente", string.IsNullOrEmpty(oProducto.OpcionExcluyente) ? (object)DBNull.Value : oProducto.OpcionExcluyente);
                    cmd.Parameters.AddWithValue("MaxOpcionesSinCosto", oProducto.MaxOpcionesSinCosto); // Agregar el nuevo valor
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }


        public bool Modificar(Producto oProducto)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_editarProducto", oConexion);
                    cmd.Parameters.AddWithValue("IdProducto", oProducto.IdProducto);
                    cmd.Parameters.AddWithValue("Nombre", oProducto.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", oProducto.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", oProducto.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", oProducto.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", oProducto.Precio);
                    cmd.Parameters.AddWithValue("OpcionesConCosto", oProducto.OpcionesConCosto);
                    cmd.Parameters.AddWithValue("OpcionesSinCosto", oProducto.OpcionesSinCosto);
                    cmd.Parameters.AddWithValue("OpcionExcluyente", oProducto.OpcionExcluyente);
                    cmd.Parameters.AddWithValue("Activo", oProducto.Activo);
                    cmd.Parameters.AddWithValue("MaxOpcionesSinCosto", oProducto.MaxOpcionesSinCosto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }


        public bool ActualizarRutaImagen(Producto oProducto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_actualizarRutaImagen", oConexion);
                    cmd.Parameters.AddWithValue("IdProducto", oProducto.IdProducto);
                    cmd.Parameters.AddWithValue("RutaImagen", oProducto.RutaImagen);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from Producto where idProducto = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

                }
                catch (Exception)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }

    }
}