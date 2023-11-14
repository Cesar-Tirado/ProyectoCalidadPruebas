using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTest.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Cumpleanos { get; set; }
        public string Distrito { get; set; }
        public int Calificacion { get; set; }
        public bool RecibirPromociones { get; set; }
    }
}
