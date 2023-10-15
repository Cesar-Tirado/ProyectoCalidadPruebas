using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoTest.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marca oMarca { get; set; }
        public Categoria oCategoria { get; set; }
        public decimal Precio { get; set; }
        public string RutaImagen { get; set; }
        public bool Activo { get; set; }
        public string base64 { get; set; }
        public string extension { get; set; }
        public string OpcionesConCosto { get; set; }
        public string OpcionesSinCosto { get;set; }
        public string OpcionExcluyente { get; set; }
        public int MaxOpcionesSinCosto { get; set; }

        
    }
}