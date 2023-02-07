using Catalogs.Model.Models;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Catalogs.Core.Controllers
{
    public class CatalogCore
    {

        private ArandaEntities db = new ArandaEntities();

        public bool CreateProduct(string productoJson) {
            try
            {
                Productos producto = JsonConvert.DeserializeObject<Productos>(productoJson);

                Productos productoDB = db.Productos.Find(producto.Id);
                if (productoDB == null)
                {
                    db.Productos.Add(producto);
                    db.SaveChanges();
                    return true;
                }
                else { 
                    return false;
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public bool UpdateProduct(string productoJson) {
            Productos producto = JsonConvert.DeserializeObject<Productos>(productoJson);
            try
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return true;
                
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductoExists(producto.Id))
                {
                    return false;
                }
                else {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                Productos producto = db.Productos.Find(id);
                if (producto != null)
                {
                    db.Productos.Remove(producto);
                    db.SaveChanges();
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetProducts(string nombre, string descripcion, int idCategoria) {

            BusquedaProducto busquedaProducto = new BusquedaProducto()
            {
                Nombre = nombre,
                Descripcion = descripcion,
                IdCategoria = idCategoria
            };
            
            var resultado = (from    productos in db.Productos.ToList()
                            where   (productos.Nombre.ToLower().ToUpper().Contains(busquedaProducto.Nombre.ToLower().ToUpper()) || busquedaProducto.Nombre == string.Empty) &
                                    (productos.Descripcion.ToLower().ToUpper().Contains(busquedaProducto.Descripcion.ToLower().ToUpper()) || busquedaProducto.Descripcion == string.Empty) &
                                    (productos.IdCategoria == busquedaProducto.IdCategoria || busquedaProducto.IdCategoria == 0 )
                            select  productos ).ToList()
                                    .Select(c => new { 
                                                        Id = c.Id,
                                                        Nombre = c.Nombre,
                                                        Descripcion = c.Descripcion,
                                                        IdCategoria = c.IdCategoria,
                                                        Categoria = new{ 
                                                            Id = c.Categoria.Id,
                                                            Descripcion = c.Categoria.Descripcion
                                                        },
                                                        Imagen = c.Imagen 
                                                    });

            var serializer = new JavaScriptSerializer();
            string lstProductos = serializer.Serialize(resultado);

            return lstProductos;
        }

        private bool ProductoExists(int id)
        {
            return db.Productos.Count(e => e.Id == id) > 0;
        }

    }
}
