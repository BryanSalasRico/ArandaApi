using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catalogs.Service.Models
{
    public class Productos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public byte[] Imagen { get; set; }

        public Categoria Categoria { get; set; }
    }
}