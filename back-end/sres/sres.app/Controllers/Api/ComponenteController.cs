using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/componente")]
    public class ComponenteController : ApiController
    {
        ComponenteLN casoLN = new ComponenteLN();

        [Route("buscarcomponente")]
        [HttpGet]
        public List<ComponenteBE> BuscarComponente(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return casoLN.ListaBusquedaComponente(new ComponenteBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("obtenercomponente")]
        [HttpGet]
        public ComponenteBE ObtenerComponente(int idcriterio, int idcaso, int idComponente)
        {
            return casoLN.getComponente(new ComponenteBE() { ID_CASO = idcaso, ID_CRITERIO = idcriterio, ID_COMPONENTE = idComponente });
        }

        [Route("guardarcomponente")]
        public bool GuardarCaso(ComponenteBE criterio)
        {
            return casoLN.GuardarComponente(criterio);
        }

        [Route("cambiarestadocomponente")]
        [HttpPost]
        public bool EliminarCaso(ComponenteBE obj)
        {
            ComponenteBE c = casoLN.EliminarComponente(obj);
            return c.OK;
        }
    }
}
