using Catalogs.Core.Controllers;
using Catalogs.Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace Catalogs.Service.Controllers
{
    public class CatalogController : ApiController
    {
        private readonly CatalogCore catalogsCore = new CatalogCore();
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        
        // GET: api/Catalogs
        public IHttpActionResult Get([FromBody] BusquedaProducto busquedaProducto)
        {
            try
            {
                string resultado = catalogsCore.GetProducts(busquedaProducto.Nombre, busquedaProducto.Descripcion, busquedaProducto.IdCategoria);
                List<Productos> lstProductos = JsonConvert.DeserializeObject<List<Productos>>(resultado);

                if (busquedaProducto.OrdenarNombre && busquedaProducto.EsAscendente) {
                    lstProductos = lstProductos.OrderBy(x => x.Nombre).ToList();
                }
                else if (busquedaProducto.OrdenarNombre && !busquedaProducto.EsAscendente)
                {
                    lstProductos = lstProductos.OrderByDescending(x => x.Nombre).ToList();
                }
                else if (!busquedaProducto.OrdenarNombre && busquedaProducto.EsAscendente) {
                    lstProductos = lstProductos.OrderBy(x => x.Categoria.Descripcion).ToList();
                }
                else {
                    lstProductos = lstProductos.OrderByDescending(x => x.Categoria.Descripcion).ToList();
                }

                return Ok( new { Data = lstProductos });
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }
        }

        // GET: api/Catalogs/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Catalogs
        public IHttpActionResult Post([FromBody] Productos producto)
        {
            try
            {
                string productoJson = serializer.Serialize(producto);
                bool response = catalogsCore.CreateProduct(productoJson);
                if (response == true)
                {
                    return Ok("Producto creado con exito");
                }
                else {
                    return BadRequest("El producto ya se registrado");
                }
            }
            catch (Exception ex) {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        // PUT: api/Catalogs/5
        public IHttpActionResult Put([FromBody] Productos producto)
        {
            try
            {
                string productoJson = serializer.Serialize(producto);
                bool response = catalogsCore.UpdateProduct(productoJson);
                if (response == true)
                {
                    return Ok("Producto Modificado con exito");
                }
                else
                {
                    return BadRequest("El producto no se encuentra registrado");
                }
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        // DELETE: api/Catalogs/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                bool response = catalogsCore.DeleteProduct(id);
                if (response == true)
                {
                    return Ok("Producto eliminado con exito");
                }
                else
                {
                    return BadRequest("El producto no se encuentra registrado");
                }
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
