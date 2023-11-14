using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProyectoTest.Logica
{
    public class UsuarioLogica
    {
        private static UsuarioLogica _instancia = null;

        public UsuarioLogica()
        {

        }

        public static UsuarioLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new UsuarioLogica();
                }

                return _instancia;
            }
        }

        public Usuario Obtener(string _correo, string _contrasena)
        {
            Usuario objeto = null;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_obtenerUsuario", oConexion);
                    cmd.Parameters.AddWithValue("Correo", _correo);
                    cmd.Parameters.AddWithValue("Contrasena", _contrasena);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) {
                        while (dr.Read()) {
                            objeto = new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"].ToString()),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Contrasena = dr["Contrasena"].ToString(),
                                EsAdministrador = Convert.ToBoolean(dr["EsAdministrador"].ToString())
                            };

                        }
                    }

                }
                catch (Exception ex)
                {
                    objeto = new Usuario();
                    objeto.Mensaje = ex.Message;
                }
            }
            return objeto;
        }

        public int Registrar(Usuario oUsuario)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrarUsuario", oConexion);
                    cmd.Parameters.AddWithValue("Nombres", oUsuario.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", oUsuario.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                    cmd.Parameters.AddWithValue("Contrasena", oUsuario.Contrasena);
                    cmd.Parameters.AddWithValue("EsAdministrador", oUsuario.EsAdministrador);
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

        public List<Cliente> ListarClientes()
        {
            List<Cliente> rptListaTiendas = new List<Cliente>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Clientes", oConexion);
                cmd.CommandType = CommandType.Text;
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaTiendas.Add(new Cliente()
                        {
                            IdCliente = Convert.ToInt32(dr["IdCliente"].ToString()),
                            Nombre = dr["Nombre"].ToString(),
                            Apellidos = dr["Apellidos"].ToString(),
                            Email = dr["Email"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Cumpleanos = dr["Cumpleanos"].ToString(),
                            Distrito = dr["Distrito"].ToString(),
                            Calificacion = Convert.ToInt32(dr["Calificacion"].ToString()),
                            RecibirPromociones = Convert.ToBoolean(dr["RecibirPromociones"].ToString()),
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

        public int RegistrarCliente(Cliente cliente)
        {
            int resultado = 0;

            using (SqlConnection conexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCliente", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Define los parámetros del procedimiento almacenado
                    cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos);
                    cmd.Parameters.AddWithValue("@Email", cliente.Email);
                    cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    cmd.Parameters.AddWithValue("@Cumpleanos", cliente.Cumpleanos);
                    cmd.Parameters.AddWithValue("@Distrito", cliente.Distrito);
                    cmd.Parameters.AddWithValue("@Calificacion", cliente.Calificacion);
                    cmd.Parameters.AddWithValue("@RecibirPromociones", cliente.RecibirPromociones);

                    // Define el parámetro de salida para obtener el ID de cliente registrado
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    // Obtiene el ID de cliente registrado
                    resultado = Convert.ToInt32(cmd.Parameters["@IdCliente"].Value);
                }
                catch (Exception)
                {
                    resultado = 0;
                }
            }

            return resultado;
        }
    }
}