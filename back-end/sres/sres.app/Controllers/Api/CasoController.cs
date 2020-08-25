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
    [RoutePrefix("api/caso")]
    public class CasoController : ApiController
    {
        CasoLN casoLN = new CasoLN();

        [Route("buscarcaso")]
        [HttpGet]
        public List<CasoBE> BuscarCaso(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return casoLN.ListaBusquedaCaso(new CasoBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("obtenercaso")]
        [HttpGet]
        public CasoBE ObtenerCaso(int idcriterio, int idcaso)
        {
            return casoLN.getCaso(new CasoBE() { ID_CASO = idcaso, ID_CRITERIO = idcriterio });
        }

        [Route("guardarcaso")]
        public bool GuardarCaso(CasoBE criterio)
        {
            return casoLN.GuardarCaso(criterio);
        }

        [Route("cambiarestadocaso")]
        [HttpPost]
        public bool EliminarCaso(CasoBE obj)
        {
            CasoBE c = casoLN.EliminarCaso(obj);
            return c.OK;
        }

        [Route("obtenercasocriterio")]
        [HttpGet]
        public List<CasoBE> ObtenerCasoCriterio(int id)
        {
            return casoLN.getCasoCriterio(new CasoBE() { ID_CRITERIO = id });
        }
    }
}
