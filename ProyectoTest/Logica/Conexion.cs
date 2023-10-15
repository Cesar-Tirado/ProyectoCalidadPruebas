using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;

namespace ProyectoTest.Logica
{
    public class Conexion
    {
        //public static string CN = "Data Source=VMI266939; Initial Catalog=LONGHORN_ADMIN; User ID=user; Password=usuario;";
        //public static string CN = "Data Source=DESKTOP-D7VGCJ9; Initial Catalog=LONGHORN_ADMIN2; User ID=user1; Password=usuario;";
        public static string CN = "Data Source=localhost;Initial Catalog=LONGHORN_ADMIN;Integrated Security=True;User ID =cesar;Password=cesar";
        //public static string CN = "Data Source=DESKTOP-D7VGCJ9;Initial Catalog=LONGHORN_ADMIN;Integrated Security=True"; // LocalAngel
    }
}