using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoTest.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdUsuario { get; set; }
        public int TotalProducto { get; set; } // Cambiado a int según el Stored Procedure
        public decimal Total { get; set; }
        public string Nombre { get; set; } // Agregado Nombre
        public string Telefono { get; set; }
        public string Correo { get; set; } // Agregado Correo
        public string Direccion { get; set; }
        public string Referencia { get; set; } // Agregado Referencia
        public string DocumentoFacturacion { get; set; } // Agregado DocumentoFacturacion
        public string FormaPago { get; set; }
        public string Estado { get; set; } // Nueva propiedad para el estado de la compra
        public string FechaTexto { get; set; }

        public List<DetalleCompra> oDetalleCompra { get; set; }

        public Compra()
        {
            oDetalleCompra = new List<DetalleCompra>();
        }
    }
}