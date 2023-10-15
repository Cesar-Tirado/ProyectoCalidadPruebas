using ProyectoTest.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ProyectoTest.Logica
{
    public class TiendasLogica
    {
        private static TiendasLogica _instancia = null;

        public TiendasLogica()
        {

        }

        public static TiendasLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new TiendasLogica();
                }

                return _instancia;
            }
        }

        public List<Tiendas> Listar()
        {

            List<Tiendas> rptListaTiendas = new List<Tiendas>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_obtenerTiendas", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaTiendas.Add(new Tiendas()
                        {
                            IdTiendas = Convert.ToInt32(dr["IdTiendas"].ToString()),
                            Descripcion = dr["Descripcion"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaTiendas;

                }
                catch (Exception ex)
                {
                    rptListaTiendas = null;
                    return rptListaTiendas;
                }
            }
        }

        public List<Tiendas> ListarTiendasActivo()
        {
            List<Tiendas> rptListaTiendas = new List<Tiendas>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("SELECT IdTiendas, Descripcion, Activo FROM Tiendas WHERE Activo = 1", oConexion);

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaTiendas.Add(new Tiendas()
                        {
                            IdTiendas = Convert.ToInt32(dr["IdTiendas"].ToString()),
                            Descripcion = dr["Descripcion"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaTiendas;

                }
                catch (Exception ex)
                {
                    rptListaTiendas = null;
                    return rptListaTiendas;
                }
            }
        }



        public bool Registrar(Tiendas oTiendas)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarTiendas", oConexion);
                    cmd.Parameters.AddWithValue("Descripcion", oTiendas.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", oTiendas.Activo);
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

        public bool Modificar(Tiendas oTiendas)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarTiendas", oConexion);
                    cmd.Parameters.AddWithValue("IdTiendas", oTiendas.IdTiendas);
                    cmd.Parameters.AddWithValue("Descripcion", oTiendas.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", oTiendas.Activo);
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

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from Tiendas where idTiendas = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }
    }
}