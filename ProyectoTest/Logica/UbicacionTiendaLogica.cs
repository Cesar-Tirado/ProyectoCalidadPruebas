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

    public class UbicacionTiendaLogica
    {

        private static UbicacionTiendaLogica _instancia = null;

        public UbicacionTiendaLogica()
        {

        }

        public static UbicacionTiendaLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new UbicacionTiendaLogica();
                }

                return _instancia;
            }
        }

        public List<UbicacionTienda> Listar()
        {
            List<UbicacionTienda> rptListaUbicacionTienda = new List<UbicacionTienda>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                //SqlCommand cmd = new SqlCommand("sp_obtenerCategoria", oConexion);
                SqlCommand cmd = new SqlCommand("SELECT * FROM UbicacionTienda", oConexion);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaUbicacionTienda.Add(new UbicacionTienda()
                        {
                            IdUbicacionTienda = Convert.ToInt32(dr["IdUbicacionTienda"].ToString()),
                            Nombre = dr["Nombre"].ToString(),
                            Distrito = dr["Distrito"].ToString()                            
                        });
                    }
                    dr.Close();

                    return rptListaUbicacionTienda;

                }
                catch (Exception ex)
                {
                    rptListaUbicacionTienda = null;
                    return rptListaUbicacionTienda;
                }
            }
        }

    }


}